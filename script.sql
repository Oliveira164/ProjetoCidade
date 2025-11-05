create database bdprojeto;

use bdprojeto;

create table Usuario(
id int primary key auto_increment,
nome varchar(40) not null,
email varchar(40) not null,
senha varchar(40) not null
);

create table Produto(
id int primary key auto_increment,
nome varchar(40) not null,
descricao varchar(80) not null,
preco decimal(10,2) not null,
quantidade int not null
);

select * from Usuario;
select * from Produto;
truncate Produto;