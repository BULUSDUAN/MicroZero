#ifndef ZERO_DEF_FRAME_H
#define ZERO_DEF_FRAME_H
#pragma once

namespace agebull
{
	namespace zero_net
	{
		namespace zero_def
		{
			/**
			 * \brief ֡����˵������
			*/
			namespace frame
			{
#define vector_str(ls,_idx_) (_idx_  <= 0 || _idx_ >= ls.size() ? nullptr : *ls[_idx_])
#define vector_int(ls,_idx_) (_idx_  <= 0 || _idx_ >= ls.size() ? 0 : atoi(*ls[_idx_]))
#define vector_int64(ls,_idx_) (_idx_  <= 0 || _idx_ >= ls.size() ? 0 : atoll(*ls[_idx_]))
#define vector_ptr(ls,_idx_) (_idx_  <= 0 || _idx_ >= ls.size() ? shared_char() : ls[_idx_])

				typedef unsigned char uchar;
				//һ����ֹ����
				constexpr uchar  general_end = '\0';
				//��չ������ֹ����
				constexpr uchar  extend_end = 0xFF;
				//��չ������ֹ����
				constexpr uchar  result_end = 0xFE;
				//ȫ�ֱ�ʶ
				constexpr uchar  global_id = 0x1;
				//վ��
				constexpr uchar  station_id = 0x2;
				//״̬
				constexpr uchar  status = 0x3;
				//����ID
				constexpr uchar  request_id = 0x4;
				//ִ�мƻ�
				constexpr uchar  plan = 0x5;
				//ִ�мƻ�
				constexpr uchar  plan_time = 0x6;
				//������֤��ʶ
				constexpr uchar  service_key = 0x7;
				//���ر�ʶ
				constexpr uchar  local_id = 0x8;

				//���÷���վ������
				constexpr uchar  station_type = 0x9;
				//���÷���ȫ�ֱ�ʶ
				constexpr uchar  call_id = 0xB;
				//���ݷ���
				constexpr uchar  data_direction = 0xC;
				//ԭ������
				constexpr uchar  original_1 = 0x10;
				//ԭ������
				constexpr uchar  original_2 = 0x11;
				//ԭ������
				constexpr uchar  original_3 = 0x12;
				//ԭ������
				constexpr uchar  original_4 = 0x13;
				//ԭ������
				constexpr uchar  original_5 = 0x14;
				//ԭ������
				constexpr uchar  original_6 = 0x15;
				//ԭ������
				constexpr uchar  original_7 = 0x16;
				//ԭ������
				constexpr uchar  original_8 = 0x17;
				//����
				constexpr uchar  command = '$';
				//����
				constexpr uchar  arg = '%';
				//֪ͨ����
				constexpr uchar  pub_title = '*';
				//֪ͨ����
				constexpr uchar  sub_title = command;
				//������������Ϣ
				constexpr uchar  context = '#';
				//������/������
				constexpr uchar  requester = '>';
				//������/������
				constexpr uchar  publisher = requester;
				//�ظ���/�˷���
				constexpr uchar  responser = '<';
				//������/�˷���
				constexpr uchar  subscriber = responser;
				//����:һ���ı�
				constexpr uchar  content = 'T';
				//����:һ���ı�
				constexpr uchar  content_text = content;
				//����:json
				constexpr uchar  content_json = 'J';
				//����:������
				constexpr uchar  content_bin = 'B';
				//���ݣ�XML
				constexpr uchar  content_xml = 'X';
				
			}
			/**
			* \brief ˵��֡����
			*/
			acl::string desc_str(bool in, uchar* desc, size_t len);
		}
	}
}
#endif