#pragma once
#ifndef _PLAN_DISPATCHER_H_
#define _PLAN_DISPATCHER_H_


#ifdef _ZERO_PLAN

#include "../stdinc.h"
#include "zero_plan.h"
#include "zero_station.h"
#include "inner_socket.h"
#include "../sqlite/plan_storage.h"

namespace agebull
{
	namespace zero_net
	{
		/**
		* \brief ��ʾ�ƻ�������ȷ���
		*/
		class plan_dispatcher final :public zero_station
		{
			friend plan_message;
			//plan_storage storage_;
			map<string, shared_ptr<inner_socket>> sockets_;
		public:
			/**
			* \brief ����
			*/
			static plan_dispatcher* instance;

			/**
			* \brief ����
			*/
			plan_dispatcher()
				:zero_station(zero_def::name::plan_dispatcher, zero_def::station_type::plan, ZMQ_ROUTER, ZMQ_PUB)
			{

			}
			/**
			* \brief ����
			*/
			plan_dispatcher(shared_ptr<station_config>& config)
				:zero_station(config, zero_def::station_type::plan, ZMQ_ROUTER, ZMQ_PUB)
			{

			}
			/**
			* \brief ����
			*/
			~plan_dispatcher() override
			{
				cout << "plan_dispatcher destory" << endl;
			}
			/**
			* \brief ����һ��֪ͨ�߳�
			*/
			static void run()
			{
				instance = new plan_dispatcher();
				boost::thread(boost::bind(launch, shared_ptr<plan_dispatcher>(instance)));
			}
		private:
			/**
			*\brief ����
			*/
			static void run(shared_ptr<station_config>& config)
			{
				instance = new plan_dispatcher(config);
				boost::thread(boost::bind(launch, shared_ptr<plan_dispatcher>(instance)));
			}
			/**
			* \brief ��Ϣ��
			*/
			static void launch(shared_ptr<plan_dispatcher>& station);


			/**
			* \brief �ƻ���ѯ
			*/
			static void run_plan_poll(plan_dispatcher* station)
			{
				station->plan_poll();
			}
			/**
			* \brief �ƻ���ѯ
			*/
			void plan_poll();

			/**
			*\brief ������Ϣ
			*/
			bool result_event(shared_ptr<plan_message>& message, vector<shared_char>& result);
			
			/**
			*\brief ֪ͨ����
			*/
			bool zero_event(zero_net_event event_type, const plan_message* message);

			/**
			* \brief ������ʼ : ������������
			*/
			void job_start(zmq_handler socket, vector<shared_char>& list, bool inner, bool old) final;
			/**
			* \brief ��������(���͵�������)
			*/
			void job_end(vector<shared_char>& list) final
			{
			}
			/**
			* \brief ִ�мƻ�
			*/
			void exec_plan(shared_ptr<plan_message>& msg);
			/**
			* \brief �ƻ�ִ�з���
			*/
			void on_plan_result(vector<shared_char>& list);
			/**
			* \brief �ƻ�ִ�з���
			*/
			void on_plan_result(shared_ptr<plan_message>& message, char state, vector<shared_char>& list);
			/**
			* \brief �ƻ�����
			*/
			bool on_plan_start(zmq_handler socket, vector<shared_char>& list);
			/**
			* \brief �ƻ�����
			*/
			void on_plan_manage(zmq_handler socket, vector<shared_char>& list);
			/**
			* \brief ִ������
			*/
			static char exec_command(const char* command, vector<shared_char>& arguments, string& json);
		};
	}
}

#endif // PLAN
#endif //!_PLAN_DISPATCHER_H_