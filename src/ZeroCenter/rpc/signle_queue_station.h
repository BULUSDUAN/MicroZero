#ifndef _ZMQ_API_SIGNLE_QUEUE_STATION_H
#define _ZMQ_API_SIGNLE_QUEUE_STATION_H
#pragma once
#include "../sqlite/queue_storage.h"
#include "zero_station.h"

namespace agebull
{
	namespace zero_net
	{
		/**
		* \brief ��ʾһ��֪ͨվ��
		*/
		class signle_queue_station final :public zero_station
		{
			queue_storage storage_;
		public:
			/**
			 * \brief ����
			 * \param name
			 */
			signle_queue_station(string name)
				: zero_station(std::move(name), zero_def::station_type::queue, ZMQ_ROUTER, ZMQ_PUB)
			{
			}

			/**
			 * \brief ����
			 * \param config
			 */
			signle_queue_station(shared_ptr<station_config>& config)
				: zero_station(config, zero_def::station_type::queue, ZMQ_ROUTER, ZMQ_PUB)
			{
			}

			/**
			* \brief ����
			*/
			virtual  ~signle_queue_station()
			{
				cout << "signle_queue_station destory" << endl;
			}

			/**
			* \brief ������ʼ : ������������
			*/
			void job_start(zmq_handler socket, vector<shared_char>& list, bool inner, bool old) final;

			/**
			* \brief ����һ��֪ͨ�߳�
			*/
			static void run(string name)
			{
				boost::thread(launch, make_shared<signle_queue_station>(name));
			}
			/**
			*\brief ����
			*/
			static void run(shared_ptr<station_config>& config)
			{
				if (config->is_state(station_state::stop))
					return;
				boost::thread(boost::bind(launch, std::make_shared<signle_queue_station>(config)));
			}
			/**
			*\brief ����һ��֪ͨ�߳�
			*/
			static void launch(shared_ptr<signle_queue_station> station);
		private:
			/**
			* \brief �ڲ�����
			*/
			bool simple_command(zmq_handler socket, vector<shared_char>& list, shared_char& description, bool inner) final;

			/**
			* \brief �ڲ�����
			*/
			static void async_replay(signle_queue_station* queue, int64 min, int64 max);
		};
	}
}
#endif//!_ZMQ_API_SIGNLE_QUEUE_STATION_H