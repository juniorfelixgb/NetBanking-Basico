
create database netbanking
go

use netbanking
go

create table Usuarios(
UsuarioID int identity constraint PK_Usuarios primary key,
NombreUsuario varchar(50) not null unique,
Email varchar(150) not null unique,
Contrasena varchar(50) not null,
Cedula varchar(11),
Nombres varchar(200) not null,
Apellidos varchar(200) not null,
FechaNacimiento date not null,
FechaRegitro datetime default getdate()
)
go

create table Monedas(
MonedaID int identity constraint PK_Monedas primary key,
MonedaNombre  varchar(150) not null unique,
Simbolo varchar(10) not null unique
)
go

create table MonedaCambios(
MonedaCambioID int identity constraint PK_MonedaCambios primary key,
MonedaIDdesde int not null constraint FK_MonedaCambios_MonedaIDdesde foreign key references Monedas(MonedaID),
MonedaIDhacia int not null constraint FK_MonedaCambios_MonedaIDhasta foreign key references Monedas(MonedaID),
Valor decimal(15,15) not null default 0.0,
Detalles varchar(max)
)
go

create table EstadoTRD(
EstadoID int identity constraint PK_EstadoTRD primary key,
Estado varchar(50) not null unique
)
go

create table Cuentas(
CuentaID int identity constraint PK_Cuentas primary key,
MonedaID int not null constraint FK_Cuentas_MonedaID foreign key references Monedas(MonedaID),
UsuarioID int not null constraint FK_Cuentas_UsuarioID foreign key references Usuarios(UsuarioID),
AlisCuenta varchar(100),
NumeroCuenta varchar(100) not null,
MontoDisponible decimal(38,2) not null default 0.0,
MontoTrancito decimal(38,2) not null default 0.0
)
go

create table Retiros(
RetiroID int identity constraint PK_Retiros primary key,
CuentaID int not null constraint FK_Retiros_CuentaID foreign key references Cuentas(CuentaID),
UsuarioID int not null constraint FK_Retiros_UsuarioID foreign key references Usuarios(UsuarioID),
EstadoID int not null constraint FK_Retiros_EstadoID foreign key references EstadoTRD(EstadoID),
AlisCuenta varchar(100),
Monto decimal(35,2) not null default 0.0,
Fecha datetime,
Detalles varchar(max)
)
go

create table Depositos(
DepositoID int identity constraint PK_Depositos primary key,
CuentaID int not null constraint FK_Depositos_CuentaID foreign key references Cuentas(CuentaID),
UsuarioID int not null constraint FK_Depositos_UsuarioID foreign key references Usuarios(UsuarioID),
EstadoID int not null constraint FK_Depositos_EstadoID foreign key references EstadoTRD(EstadoID),
AlisCuenta varchar(100),
Monto decimal(35,2) not null default 0.0,
Fecha datetime,
Detalles varchar(max)
)
go

create table Transacciones(
TransaccionID int identity constraint PK_Transacciones primary key,
RetiroID int not null constraint FK_Transacciones_RetiroID foreign key references Retiros(RetiroID),
DepositoID int not null constraint FK_Transacciones_DepositoID foreign key references Depositos(DepositoID),
EstadoID int not null constraint FK_Transacciones_EstadoID foreign key references EstadoTRD(EstadoID),
CostoTransferencia decimal(15,15) not null default 0.0,
Fecha datetime,
MonedaCambioID int constraint FK_Transacciones_MonedaCambioID foreign key references MonedaCambios(MonedaCambioID),
Detalles varchar(max)
)












