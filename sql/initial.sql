create database system;
create role orleans login password '7z9Vpm8YLS';
grant all privileges on database system to orleans;


create database trader;
create role trader login password '7z9Vpm8YLS';
grant all privileges on database trader to trader;
