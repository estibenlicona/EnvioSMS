SELECT * from cxc_firma_electronica_log2
WHERE sms_envio = 1

ALTER TABLE cxc_firma_electronica_log ADD sms_envio INT NOT NULL DEFAULT 0;
ALTER TABLE cxc_firma_electronica_log2 ADD sms_envio INT NOT NULL DEFAULT 0;


select * from cxc_cliente where cod_cli = '80171640'