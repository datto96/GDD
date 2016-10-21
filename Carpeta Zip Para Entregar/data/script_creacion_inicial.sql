/*----------- CREACION DE TABLAS ------------- */

/* TABLA DE AFILIADOS */

CREATE TABLE afiliado(
af_id BIGINT,
af_rel_id SMALLINT,
us_id BIGINT REFERENCES usuarios,
af_nombre VARCHAR(50),
af_apellido VARCHAR(50),
af_tipodoc	 VARCHAR(5),
af_numdoc BIGINT,
af_direccion VARCHAR(30),
af_telefono VARCHAR(50),
af_mail VARCHAR(30) ,
af_nacimiento DATETIME,
af_estado_civil VARCHAR(11),
af_cantidad_familiares INT,
planmed_id INT REFERENCES plan_medico,
af_sexo CHAR(1),
af_status CHAR(1),
af_fechaBaja DATETIME,
PRIMARY KEY(af_id, af_rel_id)
 );


/* TABLA DE CAMBIO DE PLAN */

CREATE TABLE logs_cambio_plan(
cambio_plan_id BIGINT PRIMARY KEY,
af_id BIGINT REFERENCES afiliado,
af_rel_id SMALLINT REFERENCES afiliado,
plan_id_ant INT REFERENCES plan_medico,
plan_id_new INT REFERENCES plan_medico,
cambio_plan_motivo VARCHAR(100),
cambio_plan_fecha DATETIME
);


/* TABLA DE PLAN MEDICO */

CREATE TABLE plan_medico(
planmed_id INT PRIMARY KEY,
plan_cuota INT,
plan_nombre VARCHAR(30),
plan_precio_bono DECIMAL(5,2)
);

/* TABLA DE SERVICIOS POR PLANES */

CREATE TABLE servicios_por_planes(
planmed_id INT REFERENCES plan_medico,
serv_id INT REFERENCES servicios
);


/* TABLA DE SERVICIOS */

CREATE TABLE servicios(
serv_id INT PRIMARY KEY,
serv_desc VARCHAR(100)
);

/* TABLA DE REGISTRO DE COMPRA */

CREATE TABLE registro_compra(
compra_id INT PRIMARY KEY,
af_id BIGINT REFERENCES afiliado,
af_rel_id SMALLINT REFERENCES afiliado,
compra_cantidad INT,
compra_monto DECIMAL(7,2),
compra_estado VARCHAR(10)
);


/* TABLA DE BONO */

CREATE TABLE bono(
bono_id INT PRIMARY KEY,
compra_id INT REFERENCES registro_compra,
planmed_id INT REFERENCES plan_medico,
cons_id INT REFERENCES consulta_medica,
af_id BIGINT REFERENCES afiliado,
af_rel_id SMALLINT REFERENCES afiliado
);

/* TABLA DE CONSULTA MEDICA */

CREATE TABLE consulta_medica(
cons_id INT PRIMARY KEY,
turno_id INT REFERENCES turnos,
cons_hora_llegada DATETIME,
cons_hora_turno DATETIME,
cons_sintomas VARCHAR(200),
cons_diagnostico VARCHAR(200),
cons_estado CHAR(1),
bono_id INT REFERENCES bono
);


/* TABLA DE TURNOS */

CREATE TABLE turnos(
turno_id INT PRIMARY KEY,
turno_hora DATETIME,
turno_estado CHAR(1),
af_id BIGINT REFERENCES afiliado,
prof_id BIGINT REFERENCES profesional,
esp_id INT REFERENCES especialidad,
);


/* TABLA DE CANCELACIONES */

CREATE TABLE cancelacion(
cancel_id  INT PRIMARY KEY,
cancel_tipo CHAR(1),
cancel_motivo VARCHAR(30),
turno_id INT REFERENCES turnos,
);


/* TABLA DE AGENDA DEL PROFESIONAL */

CREATE TABLE agenda_profesional(
agenda_id BIGINT PRIMARY KEY,
prof_id BIGINT REFERENCES profesional,
agenda_fecha_inicial DATETIME,
agenda_fecha_final DATETIME,
esp_id INT REFERENCES especialidad
);

/* TABLA DE TIPO DE ESPECIALIDADES */

CREATE TABLE tipo_especialidades(
tipoEsp_id INT PRIMARY KEY,
tipoEsp_descripcion VARCHAR(50)
);

/* TABLA DE ESPECIALIDADES */
CREATE TABLE especialidad(
esp_id INT PRIMARY KEY,
esp_descripcion VARCHAR(100),
tipoEsp_id INT REFERENCES tipo_especialidades
);

/* TABLA DE ESPECIALIDAD POR PROFESIONAL */

CREATE TABLE especialidad_por_profesional(
prof_id BIGINT REFERENCES profesional,
esp_id INT REFERENCES especialidad
);


/* TABLA DE PROFESIONALES */

CREATE TABLE profesional(
prof_id BIGINT PRIMARY KEY,
us_id BIGINT REFERENCES usuarios,
prof_nombre VARCHAR(50),
prof_apellido VARCHAR(50),
prof_tipodoc VARCHAR(5),
prof_numdoc BIGINT,
prof_direccion VARCHAR(50),
prof_telefono VARCHAR(20),
prof_mail VARCHAR(40),
prof_nacimiento DATETIME,
prof_matricula BIGINT,
prof_sexo CHAR(1)
);

/* TABLA DE PERIODO BAJA DE TURNO */
CREATE TABLE periodo_baja(
periodo_id INT PRIMARY KEY,
periodo_desde DATE,
periodo_hasta DATE,
prof_id BIGINT REFERENCES profesional
);


/* TABLA DE USUARIOS */

CREATE TABLE usuarios(
us_id BIGINT PRIMARY KEY IDENTITY(1,1),
us_username VARCHAR(30),
us_password CHAR(64),
us_login_fail SMALLINT,
us_status Char(1)
);

/* TABLA DE ROLES POR USUARIO */

CREATE TABLE rol_por_usuarios(
us_id INT  REFERENCES usuarios,
rol_id INT  REFERENCES rol
);

/* TABLA DE ROLES */

CREATE TABLE rol(
rol_id INT PRIMARY KEY,
rol_nombre VARCHAR(30),
rol_status INT
);

/* TABLA DE FUNCIONALIDADES POR ROL */

CREATE TABLE funcionalidad_por_rol(
rol_id INT  REFERENCES rol,
fun_id INT  REFERENCES funcionalidad
);

/* TABLA DE FUNCIONALIDADES */

CREATE TABLE funcionalidad(
fun_id INT PRIMARY KEY,
fun_nombre VARCHAR(30),
fun_descripcion VARCHAR(100)
);


/* -------------------- CREACION DE PKs Compuestas --------------------- */

ALTER TABLE afiliado ADD CONSTRAINT PK_afiliado_con_rel
  PRIMARY KEY clustered (af_id, af_rel_id);


ALTER TABLE servicios_por_planes ADD CONSTRAINT PK_servicio_plan
  PRIMARY KEY clustered (planmed_id,serv_id);


ALTER TABLE especialidad_por_profesional ADD CONSTRAINT PK_especialidad_profesional
  PRIMARY KEY clustered (prof_id, esp_id);


ALTER TABLE rol_por_usuarios ADD CONSTRAINT PK_roles_usuario
  PRIMARY KEY clustered (id_usuario,rol_id);


ALTER TABLE funcionalidad_por_rol ADD CONSTRAINT PK_funcionalidad_rol
  PRIMARY KEY clustered (rol_id, fun_id);
  
  
/* ------------------ CREACION DE FKs (Por favor en mismo orden de creacion de tablas para no olvidar nada) --------------------*/

ALTER TABLE afiliado add constraint FK_afi_usuario
	foreign key (us_id) references usuarios (us_id);

ALTER TABLE afiliado add constraint FK_plan_med
	foreign key (planmed_id) references plan_medico (planmed_id);
	
/* NO ESTOY SEGURO DE QUE ESTE BIEN

ALTER TABLE logs_cambio_plan add constraint FK_afi
	foreign key (af_id) references afiliado (af_id);
	
ALTER TABLE logs_cambio_plan add constraint FK_afi_rel
        foreign key (af_rel_id) references afiliado (af_rel_id);
	
*/

ALTER TABLE logs_cambio_plan add constraint FK_plan_ant
        foreign key (plan_id_ant) references plan_medico (planmed_id);
	
ALTER TABLE logs_cambio_plan add constraint FK_plan_new
        foreign key (plan_id_new) references plan_medico (planmed_id);
	
/*

ALTER TABLE registro_compra add constraint FK_compra_afi
        foreign key (af_id) references afiliado (af_id);
	
ALTER TABLE registro_compra add constraint FK_compra_afi_rel
        foreign key (af_rel_id) references afiliado (af_rel_id);
	
*/

ALTER TABLE bono add constraint FK_compra_bono
        foreign key (compra_id) references registro_compra (compra_id);
	
ALTER TABLE bono add constraint FK_plan_bono
        foreign key (planmed_id) references plan_medico (planmed_id);	
	
ALTER TABLE bono add constraint FK_bono_consulta
        foreign key (cons_id) references consulta_medica (cons_id);
	
/*

ALTER TABLE bono add constraint FK_afi_bono
        foreign key (af_id) references afiliado (af_id);
	
ALTER TABLE bono add constraint FK_afi_rel_bono
        foreign key (af_rel_id) references afiliado (af_rel_id);
	
*/

ALTER TABLE consulta_medica add constraint FK_consulta_turno
        foreign key (turno_id) references turnos (turno_id);
	
ALTER TABLE consulta_medica add constraint FK_consulta_bono
        foreign key (bono_id) references registro_compra (bono_id);	

ALTER TABLE turnos add constraint FK_turno_afi
        foreign key (afi_id) references afiliado (afi_id);
	
ALTER TABLE turnos add constraint FK_turno_prof
        foreign key (prof_id) references profesional (prof_id);	
	
ALTER TABLE turnos add constraint FK_turno_cancel
        foreign key (cancel_id) references cancelacion (cancel_id);

ALTER TABLE turnos add constraint FK_turno_esp
        foreign key (esp_id) references especialidad (esp_id);

ALTER TABLE agenda_profesional add constraint FK_agenda_prof
        foreign key (prof_id) references profesional (prof_id);	
	
ALTER TABLE especialidad add constraint FK_especialidad_tipo
        foreign key (tipoEsp_id) references tipo_especialidades (tipoEsp_id);

ALTER TABLE profesional add constraint FK_prof_us
        foreign key (us_id) references usuarios (us_id);
	
ALTER TABLE periodo_baja add constraint FK_baja_prof
        foreign key (prof_id) references profesional (prof_id);	
	





