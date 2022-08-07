CREATE SEQUENCE userid_seq START WITH 1000000000 INCREMENT BY 1; 

create table user
(
	userid bigint primary key not null DEFAULT NEXTVAL('userid_seq'),
	appid varchar(255) not null,
	name varchar(100),
	phonenumber varchar(100),
	email varchar(100),
	verificationcode varchar(100)
);