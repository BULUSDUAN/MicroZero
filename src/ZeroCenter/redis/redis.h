#ifndef AGEBULL_REDIS_H
#define AGEBULL_REDIS_H
#pragma once

#ifdef _ZERO_REDIS
#include "../stdinc.h"
#include "../cfg/json_config.h"
#include "../ext/shared_char.h"
namespace agebull
{
	class redis_live_scope;
	class redis_trans_scope;
	/**
	* \brief ����Redis ��û������ʱ,����ͨʹ��һ��,����ʱ����begin_trans,�ύ����commit,���˵���rollback,�ұ���ɶԵ���
	*/
	class trans_redis
	{
		friend class redis_live_scope;
		friend class redis_trans_scope;


		/**
		* \brief ��������Ĵ���
		*/
		int m_trans_num;
		/**
		* \brief �Ƿ��л��˵ĵ���,��������м�ʧ��,������ʽ����Ϊ������
		*/
		bool m_failed;
		/**
		* \brief ���һ�β����Ƿ�ɹ�
		*/
		bool m_last_status;
		/**
		* \brief acl��redis�ͻ��˶���
		*/
		acl::redis_client* m_redis_client;
		/**
		* \brief acl��redis�������
		*/
		acl::redis* m_redis_cmd;
		/**
		* \brief acl��redis�������
		*/
		int m_cur_db_;
		/**
		* \brief �������޸ĵ�����
		*/
		map<acl::string, int> m_modifies;
		/**
		* \brief ���ػ������
		*/
		map<acl::string, acl::string> m_local_values;
	public:
		/**
		* \brief �����ļ��е�redis�ĵ�ַ
		*/
		static const char* redis_ip()
		{
			return global_config::redis_addr;
		}

		/**
		* \brief �����ļ��е�redis��db
		*/
		int cur_db() const
		{
			return m_cur_db_;
		}
		/**
		* \brief ����
		*/
		trans_redis(int db);
		/**
		* \brief ����
		*/
		~trans_redis();
		/**
		* \brief ȡ�õ�ǰ�߳������ĵ�����Redis����
		* @return ��ǰ�߳������ĵĲ�������
		*/
		static trans_redis& get_context();
		/**
		* \brief ȡ�õ�ǰ�߳������ĵ�����Redis����
		* @return ��ǰ�߳������ĵĲ�������
		*/
		static trans_redis* get_current();
		/**
		* \brief ���ɵ�ǰ�߳������ĵ�����Redis����
		*/
		static bool open_context();
		/**
		* \brief ���ɵ�ǰ�߳������ĵ�����Redis����
		*/
		static	bool open_context(int db);
		/**
		* \brief �رյ�ǰ�߳������ĵ�����Redis����
		*/
		static void close_context();

		/**
		* ѡ�� redis-server �е����ݿ� ID
		* SELECT command to select the DB id in redis-server
		* @param dbnum {int} redis ���ݿ� ID
		*  the DB id
		* @return {bool} �����Ƿ�ɹ�
		*  return true if success, or false for failed.
		*/
		bool select(int dbnum);
		/**
		* \brief ��������
		* @return ��ǰ�߳������ĵĲ�������
		*/
		static trans_redis& begin_trans();
		/**
		* \brief �ύ����,�������������������ĵط�����,ֻ�Ǽ����������ô���,���һ�ε���(��Ӧ�������begin_trans)ʱ,���֮ǰm_failed������,�ڲ����ǻ����rollback,����ignore_failed����Ϊtrue
		* @param {bool} ignore_failed ����m_failed������,�����Եĵ����ύ
		*/
		static void end_trans(bool ignore_failed = false);
		/**
		* \brief ���ó���
		*/
		static void set_failed();
		/**
		* \brief ���һ�β����Ƿ�ɹ�
		*/
		bool last_status() const;
	private:
		/**
		* \brief �ύ����
		*/
		void commit_inner();
	public:
		bool get(const char*, acl::string&);
		void set(const char*, const char*);
		void set(const char*, const char*, size_t);
		void set(const char*, acl::string&);
		acl::redis* operator->() const;
		acl::string read_str_from_redis(const char* key);
		template<class TArg1>
		acl::string read_str_from_redis(const char* key_fmt, TArg1 arg);

		template<class TArg1, class TArg2>
		acl::string read_str_from_redis(const char* key_fmt, TArg1 arg1, TArg2 arg2);

		template<class TArg1, class TArg2, class TArg3>
		acl::string read_str_from_redis(const char* key_fmt, TArg1 arg1, TArg2 arg2, TArg3 arg3);

		std::vector<acl::string> find_redis_keys(const char* find_key) const;
		template<class TArg1>
		std::vector<acl::string> find_redis_keys(const char* key_fmt, TArg1 arg);

		template<class TArg1, class TArg2>
		std::vector<acl::string> find_redis_keys(const char* key_fmt, TArg1 arg1, TArg2 arg2);

		template<class TArg1, class TArg2, class TArg3>
		std::vector<acl::string> find_redis_keys(const char* key_fmt, TArg1 arg1, TArg2 arg2, TArg3 arg3);

		std::vector<acl::string*> find_from_redis(const char* find_key);
		template<class TArg1>
		std::vector<acl::string*> find_from_redis(const char* key_fmt, TArg1 arg);

		template<class TArg1, class TArg2>
		std::vector<acl::string*> find_from_redis(const char* key_fmt, TArg1 arg1, TArg2 arg2);

		template<class TArg1, class TArg2, class TArg3>
		std::vector<acl::string*> find_from_redis(const char* key_fmt, TArg1 arg1, TArg2 arg2, TArg3 arg3);
		acl::string read_from_redis(const char* key);
		template<class TArg1>
		acl::string read_from_redis(const char* key_fmt, TArg1 arg);

		template<class TArg1, class TArg2>
		acl::string read_from_redis(const char* key_fmt, TArg1 arg1, TArg2 arg2);

		template<class TArg1, class TArg2, class TArg3>
		acl::string read_from_redis(const char* key_fmt, TArg1 arg1, TArg2 arg2, TArg3 arg3);

		void delete_from_redis(const char* find_key) const;

		template<class TArg1>
		void delete_from_redis(const char* key_fmt, TArg1 arg);

		template<class TArg1, class TArg2>
		void delete_from_redis(const char* key_fmt, TArg1 arg1, TArg2 arg2);

		template<class TArg1, class TArg2, class TArg3>
		void delete_from_redis(const char* key_fmt, TArg1 arg1, TArg2 arg2, TArg3 arg3);

		size_t incr_redis(const char* key) const;
		template<class TArg1>
		size_t incr_redis(const char* key_fmt, TArg1 arg);

		template<class TArg1, class TArg2>
		size_t incr_redis(const char* key_fmt, TArg1 arg1, TArg2 arg2);

		template<class TArg1, class TArg2, class TArg3>
		size_t incr_redis(const char* key_fmt, TArg1 arg1, TArg2 arg2, TArg3 arg3);
		void write_to_redis(const char* key, const char* bin, size_t len);
		void write_json_to_redis(const char* key, const char* json);
		template<class TArg1>
		void write_to_redis(const char* bin, size_t len, const char* key_fmt, TArg1 arg);

		template<class TArg1, class TArg2>
		void write_to_redis(const char* bin, size_t len, const char* key_fmt, TArg1 arg1, TArg2 arg2);

		template<class TArg1, class TArg2, class TArg3>
		void write_to_redis(const char* bin, size_t len, const char* key_fmt, TArg1 arg1, TArg2 arg2, TArg3 arg3);


		acl::string read_first_from_redis(const char* key);
		template<class TArg1>
		acl::string read_first_from_redis(const char* key_fmt, TArg1 arg);

		template<class TArg1, class TArg2>
		acl::string read_first_from_redis(const char* key_fmt, TArg1 arg1, TArg2 arg2);

		template<class TArg1, class TArg2, class TArg3>
		acl::string read_first_from_redis(const char* key_fmt, TArg1 arg1, TArg2 arg2, TArg3 arg3);


		bool unlock_from_redis(const char* key) const;
		template<class TArg1>
		bool unlock_from_redis(const char* key_fmt, TArg1 arg);

		template<class TArg1, class TArg2>
		bool unlock_from_redis(const char* key_fmt, TArg1 arg1, TArg2 arg2);

		template<class TArg1, class TArg2, class TArg3>
		bool unlock_from_redis(const char* key_fmt, TArg1 arg1, TArg2 arg2, TArg3 arg3);

		bool lock_from_redis(const char* key) const;
		template<class TArg1>
		bool lock_from_redis(const char* key_fmt, TArg1 arg);

		template<class TArg1, class TArg2>
		bool lock_from_redis(const char* key_fmt, TArg1 arg1, TArg2 arg2);

		template<class TArg1, class TArg2, class TArg3>
		bool lock_from_redis(const char* key_fmt, TArg1 arg1, TArg2 arg2, TArg3 arg3);

		bool get_hash(const char* key, const char* sub_key, acl::string& vl) const;
		bool set_hash(const char* key, const char* sub_key, const char* vl) const;
		bool del_hash(const char* key, const char* sub_key) const;
		bool get_hash(const char* key, std::map<acl::string, acl::string>& vl) const;

		bool set_hash_val(const char* key, const char* sub_key, const agebull::zero_net::shared_char& ptr) const
		{
			return m_redis_cmd->hset(key, sub_key, *ptr, ptr.size()) >= 0;
		}
		void set_hash_val(const char* key, const char* sub_key, int64 number)
		{
			char buf[32];
			sprintf(buf, "%lld", number);
			m_redis_cmd->hset(key, sub_key, buf);
		}
		void set_hash_val(const char* key, const char* sub_key, int64_t number)
		{
			char buf[32];
			sprintf(buf, "%lld", number);
			m_redis_cmd->hset(key, sub_key, buf);
		}
		void set_hash_val(const char* key, const char* sub_key,uint64 number)
		{
			char buf[32];
			sprintf(buf, "%llu", number);
			m_redis_cmd->hset(key, sub_key, buf);
		}
		void set_hash_val(const char* key, const char* sub_key, bool number)
		{
			m_redis_cmd->hset(key, sub_key, number ? "1":"0");
		}
		void set_hash_val(const char* key, const char* sub_key, int number)
		{
			char buf[32];
			sprintf(buf, "%d", number);
			m_redis_cmd->hset(key, sub_key, buf);
		}
		void set_hash_val(const char* key, const char* sub_key, const char* str)
		{
			m_redis_cmd->hset(key, sub_key, str);
		}
		void set_hash_val(const char* key, const char* sub_key, uint number)
		{
			char buf[32];
			sprintf(buf, "%u", number);
			m_redis_cmd->hset(key, sub_key, buf);
		}
		void set_hash_val(const char* key, const char* sub_key, ulong number)
		{
			char buf[32];
			sprintf(buf, "%lu", number);
			m_redis_cmd->hset(key, sub_key, buf);
		}

		bool get_hash_val(const char* key, const char* sub_key, agebull::zero_net::shared_char& ptr) const
		{
			acl::string val;
			if (!m_redis_cmd->hget(key, sub_key, val))
			{
				return false;
			}
			ptr = val;
			return true;
		}
		bool get_hash_val(const char* key, const char* sub_key, bool& num) const
		{
			acl::string val;
			if (!m_redis_cmd->hget(key, sub_key, val))
			{
				return false;
			}
			num ="1" == val;
			return true;
		}
		bool get_hash_val(const char* key, const char* sub_key, int& num) const
		{
			acl::string val;
			if (!m_redis_cmd->hget(key, sub_key, val))
			{
				return false;
			}
			num = atoi(val.c_str());
			return true;
		}
		bool get_hash_val(const char* key, const char* sub_key, uint& num) const
		{
			acl::string val;
			if (!m_redis_cmd->hget(key, sub_key, val))
			{
				return false;
			}
			num = atoi(val.c_str());
			return true;
		}
		bool get_hash_val(const char* key, const char* sub_key, long& num) const
		{
			acl::string val;
			if (!m_redis_cmd->hget(key, sub_key, val))
			{
				return false;
			}
			num = atol(val.c_str());
			return true;
		}
		bool get_hash_val(const char* key, const char* sub_key, ulong& num) const
		{
			acl::string val;
			if (!m_redis_cmd->hget(key, sub_key, val))
			{
				return false;
			}
			num = atol(val.c_str());
			return true;
		}
		bool get_hash_val(const char* key, const char* sub_key, int64& num) const
		{
			acl::string val;
			if (!m_redis_cmd->hget(key, sub_key, val))
			{
				return false;
			}
			num = atoll(val.c_str());
			return true;
		}
		bool get_hash_val(const char* key, const char* sub_key, uint64& num) const
		{
			acl::string val;
			if (!m_redis_cmd->hget(key, sub_key, val))
			{
				return false;
			}
			num = atoll(val.c_str());
			return true;
		}
		agebull::zero_net::shared_char get_hash_ptr(const char* key, const char* sub_key) const
		{
			acl::string val;
			if (!m_redis_cmd->hget(key, sub_key, val))
			{
				return agebull::zero_net::shared_char();
			}
			return agebull::zero_net::shared_char(val);
		}
		long get_hash_num(const char* key, const char* sub_key) const
		{
			acl::string val;
			if (!m_redis_cmd->hget(key, sub_key, val))
			{
				return 0L;
			}
			return atol(val.c_str());
		}
		long long get_hash_int64(const char* key, const char* sub_key) const
		{
			acl::string val;
			if (!m_redis_cmd->hget(key, sub_key, val))
			{
				return 0L;
			}
			return atoll(val.c_str());
		}

	};

	/**
	* \brief Redis��ǰ�����Ķ������淶Χ
	*/
	class redis_live_scope
	{
		trans_redis* redis_;
		bool open_by_me_;
		int old_db_;
	public:
		/**
		* \brief ����
		*/
		redis_live_scope();
		/**
		* \brief �����ȡ
		*/
		acl::redis* operator->() const
		{
			return redis_->m_redis_cmd;
		}
		/**
		* \brief �����ȡ
		*/
		trans_redis* redis() const
		{
			return redis_;
		}
		/**
		* \brief ����
		*/
		redis_live_scope(int db);

		/**
		* \brief ����
		*/
		~redis_live_scope();
	};
	/**
	* \brief �Զ��������ύ������Χ
	*/
	class redis_trans_scope
	{
	public:
		/**
		* \brief ����
		*/
		redis_trans_scope();

		/**
		* \brief ����
		*/
		~redis_trans_scope();
	};

	inline bool ping_redis()
	{
		return trans_redis::get_context()->ping();
	}
	inline acl::string read_str_from_redis(const char* key)
	{
		return trans_redis::get_context().read_str_from_redis(key);
	}
	template<class TArg1>
	inline acl::string read_str_from_redis(const char* key_fmt, TArg1 arg)
	{
		char key[256];
		sprintf(key, key_fmt, arg);
		return read_str_from_redis(key);
	}
	template<class TArg1, class TArg2>
	inline acl::string read_str_from_redis(const char* key_fmt, TArg1 arg1, TArg2 arg2)
	{
		char key[256];
		sprintf(key, key_fmt, arg1, arg2);
		return read_str_from_redis(key);
	}
	template<class TArg1, class TArg2, class TArg3>
	inline acl::string read_str_from_redis(const char* key_fmt, TArg1 arg1, TArg2 arg2, TArg3 arg3)
	{
		char key[256];
		sprintf(key, key_fmt, arg1, arg2, arg3);
		return read_str_from_redis(key);
	}

	inline std::vector<acl::string> find_redis_keys(const char* key)
	{
		return trans_redis::get_context().find_redis_keys(key);
	}
	template<class TArg1>
	inline std::vector<acl::string> find_redis_keys(const char* key_fmt, TArg1 arg)
	{
		char key[256];
		sprintf(key, key_fmt, arg);
		return find_redis_keys(key);
	}
	template<class TArg1, class TArg2>
	inline std::vector<acl::string> find_redis_keys(const char* key_fmt, TArg1 arg1, TArg2 arg2)
	{
		char key[256];
		sprintf(key, key_fmt, arg1, arg2);
		return find_redis_keys(key);
	}
	template<class TArg1, class TArg2, class TArg3>
	inline std::vector<acl::string> find_redis_keys(const char* key_fmt, TArg1 arg1, TArg2 arg2, TArg3 arg3)
	{
		char key[256];
		sprintf(key, key_fmt, arg1, arg2, arg3);
		return find_redis_keys(key);
	}

	inline std::vector<acl::string*> find_from_redis(const char* key)
	{
		return trans_redis::get_context().find_from_redis(key);
	}
	template<class TArg1>
	inline std::vector<acl::string*> find_from_redis(const char* key_fmt, TArg1 arg)
	{
		char key[256];
		sprintf(key, key_fmt, arg);
		return find_from_redis(key);
	}
	template<class TArg1, class TArg2>
	inline std::vector<acl::string*> find_from_redis(const char* key_fmt, TArg1 arg1, TArg2 arg2)
	{
		char key[256];
		sprintf(key, key_fmt, arg1, arg2);
		return find_from_redis(key);
	}
	template<class TArg1, class TArg2, class TArg3>
	inline std::vector<acl::string*> find_from_redis(const char* key_fmt, TArg1 arg1, TArg2 arg2, TArg3 arg3)
	{
		char key[256];
		sprintf(key, key_fmt, arg1, arg2, arg3);
		return find_from_redis(key);
	}
	inline acl::string read_from_redis(const char* key)
	{
		return trans_redis::get_context().read_from_redis(key);
	}
	template<class TArg1>
	inline acl::string read_from_redis(const char* key_fmt, TArg1 arg)
	{
		char key[256];
		sprintf(key, key_fmt, arg);
		return read_from_redis(key);
	}
	template<class TArg1, class TArg2>
	inline acl::string read_from_redis(const char* key_fmt, TArg1 arg1, TArg2 arg2)
	{
		char key[256];
		sprintf(key, key_fmt, arg1, arg2);
		return read_from_redis(key);
	}
	template<class TArg1, class TArg2, class TArg3>
	inline acl::string read_from_redis(const char* key_fmt, TArg1 arg1, TArg2 arg2, TArg3 arg3)
	{
		char key[256];
		sprintf(key, key_fmt, arg1, arg2, arg3);
		return read_from_redis(key);
	}

	inline void delete_from_redis(const char* key)
	{
		return trans_redis::get_context().delete_from_redis(key);
	}

	template<class TArg1>
	inline void delete_from_redis(const char* key_fmt, TArg1 arg)
	{
		char key[256];
		sprintf(key, key_fmt, arg);
		delete_from_redis(key);
	}
	template<class TArg1, class TArg2>
	inline void delete_from_redis(const char* key_fmt, TArg1 arg1, TArg2 arg2)
	{
		char key[256];
		sprintf(key, key_fmt, arg1, arg2);
		delete_from_redis(key);
	}
	template<class TArg1, class TArg2, class TArg3>
	inline void delete_from_redis(const char* key_fmt, TArg1 arg1, TArg2 arg2, TArg3 arg3)
	{
		char key[256];
		sprintf(key, key_fmt, arg1, arg2, arg3);
		delete_from_redis(key);
	}

	inline size_t incr_redis(const char* key)
	{
		return trans_redis::get_context().incr_redis(key);
	}
	template<class TArg1>
	inline size_t incr_redis(const char* key_fmt, TArg1 arg)
	{
		char key[256];
		sprintf(key, key_fmt, arg);
		return incr_redis(key);
	}
	template<class TArg1, class TArg2>
	inline size_t incr_redis(const char* key_fmt, TArg1 arg1, TArg2 arg2)
	{
		char key[256];
		sprintf(key, key_fmt, arg1, arg2);
		return incr_redis(key);
	}
	template<class TArg1, class TArg2, class TArg3>
	inline size_t incr_redis(const char* key_fmt, TArg1 arg1, TArg2 arg2, TArg3 arg3)
	{
		char key[256];
		sprintf(key, key_fmt, arg1, arg2, arg3);
		return incr_redis(key);
	}
	inline void write_to_redis(const char* key, const char* bin, size_t len)
	{
		return trans_redis::get_context().write_to_redis(key, bin, len);
	}
	inline void write_json_to_redis(const char* key, const char* json)
	{
		return trans_redis::get_context().write_json_to_redis(key, json);
	}
	template<class TArg1>
	inline void write_to_redis(const char* bin, size_t len, const char* key_fmt, TArg1 arg)
	{
		char key[256];
		sprintf(key, key_fmt, arg);
		write_to_redis(key, bin, len);
	}
	template<class TArg1, class TArg2>
	inline void write_to_redis(const char* bin, size_t len, const char* key_fmt, TArg1 arg1, TArg2 arg2)
	{
		char key[256];
		sprintf(key, key_fmt, arg1, arg2);
		write_to_redis(key, bin, len);
	}
	template<class TArg1, class TArg2, class TArg3>
	inline void write_to_redis(const char* bin, size_t len, const char* key_fmt, TArg1 arg1, TArg2 arg2, TArg3 arg3)
	{
		char key[256];
		sprintf(key, key_fmt, arg1, arg2, arg3);
		write_to_redis(key, bin, len);
	}


	inline acl::string read_first_from_redis(const char* key)
	{
		return trans_redis::get_context().read_first_from_redis(key);
	}
	template<class TArg1>
	inline acl::string read_first_from_redis(const char* key_fmt, TArg1 arg)
	{
		char key[256];
		sprintf(key, key_fmt, arg);
		return read_first_from_redis(key);
	}
	template<class TArg1, class TArg2>
	inline acl::string read_first_from_redis(const char* key_fmt, TArg1 arg1, TArg2 arg2)
	{
		char key[256];
		sprintf(key, key_fmt, arg1, arg2);
		return read_first_from_redis(key);
	}
	template<class TArg1, class TArg2, class TArg3>
	inline acl::string read_first_from_redis(const char* key_fmt, TArg1 arg1, TArg2 arg2, TArg3 arg3)
	{
		char key[256];
		sprintf(key, key_fmt, arg1, arg2, arg3);
		return read_first_from_redis(key);
	}

	inline int append_redis(const char* key, const char* value)
	{
		return trans_redis::get_context()->append(key, value);
	}


	inline bool unlock_from_redis(const char* key)
	{
		return trans_redis::get_context().unlock_from_redis(key);
	}
	template<class TArg1>
	inline bool unlock_from_redis(const char* key_fmt, TArg1 arg)
	{
		char key[256];
		sprintf(key, key_fmt, arg);
		return unlock_from_redis(key);
	}
	template<class TArg1, class TArg2>
	inline bool unlock_from_redis(const char* key_fmt, TArg1 arg1, TArg2 arg2)
	{
		char key[256];
		sprintf(key, key_fmt, arg1, arg2);
		return unlock_from_redis(key);
	}
	template<class TArg1, class TArg2, class TArg3>
	inline bool unlock_from_redis(const char* key_fmt, TArg1 arg1, TArg2 arg2, TArg3 arg3)
	{
		char key[256];
		sprintf(key, key_fmt, arg1, arg2, arg3);
		return unlock_from_redis(key);
	}

	inline bool lock_from_redis(const char* key)
	{
		return trans_redis::get_context().lock_from_redis(key);
	}
	template<class TArg1>
	inline bool lock_from_redis(const char* key_fmt, TArg1 arg)
	{
		char key[256];
		sprintf(key, key_fmt, arg);
		return lock_from_redis(key);
	}
	template<class TArg1, class TArg2>
	inline bool lock_from_redis(const char* key_fmt, TArg1 arg1, TArg2 arg2)
	{
		char key[256];
		sprintf(key, key_fmt, arg1, arg2);
		return lock_from_redis(key);
	}
	template<class TArg1, class TArg2, class TArg3>
	inline bool lock_from_redis(const char* key_fmt, TArg1 arg1, TArg2 arg2, TArg3 arg3)
	{
		char key[256];
		sprintf(key, key_fmt, arg1, arg2, arg3);
		return lock_from_redis(key);
	}
}
#endif
#endif