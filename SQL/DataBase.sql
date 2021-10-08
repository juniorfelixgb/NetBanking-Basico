
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
NumeroCuenta varchar(100) not null unique,
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
go


alter table Cuentas add constraint CHK_MondoDiponible_minimo check (MontoDisponible >= 0)
go
alter table Cuentas add constraint UQ_NumeroCuenta unique (NumeroCuenta)
go
 
CREATE PROCEDURE Transferir  
    @NumeroCuentaRetiro VARCHAR(100) , 
    @NumeroCuentaDeposito VARCHAR(100),  
AS  
    -- Detect whether the procedure was called  
    -- from an active transaction and save  
    -- that for later use.  
    -- In the procedure, @TranCounter = 0  
    -- means there was no active transaction  
    -- and the procedure started one.  
    -- @TranCounter > 0 means an active  
    -- transaction was started before the   
    -- procedure was called.  
	--- Detectar si se llamó al procedimiento
 --   - de una transacción activa y guardar
 --   - eso para uso posterior.
 --   - En el procedimiento, @TranCounter = 0
 --   - significa que no hubo una transacción activa
 --   - y el procedimiento inició uno.
 --   - @TranCounter> 0 significa un activo
 --   - la transacción se inició antes de la
 --   - se llamó al procedimiento.
    DECLARE @TranCounter INT;  
    SET @TranCounter = @@TRANCOUNT;  
    IF @TranCounter > 0  
        -- Procedure called when there is  
        -- an active transaction.  
        -- Create a savepoint to be able  
        -- to roll back only the work done  
        -- in the procedure if there is an  
        -- error.
		--- Procedimiento llamado cuando hay
  --      - una transacción activa.
  --      - Crea un punto de guardado para poder
  --      - para revertir solo el trabajo realizado
  --      - en el procedimiento si hay un
  --      -- error.
        SAVE TRANSACTION ProcedureSave;  
    ELSE  
        -- Procedure must start its own  
        -- transaction.  
		--- El procedimiento debe comenzar por sí mismo
  --      - transacción.
        BEGIN TRANSACTION;  
    -- Modify database.  
    BEGIN TRY  
        DELETE HumanResources.JobCandidate  
            WHERE JobCandidateID = @InputCandidateID;  
        -- Get here if no errors; must commit  
        -- any transaction started in the  
        -- procedure, but not commit a transaction  
        -- started before the transaction was called.  
        IF @TranCounter = 0  
            -- @TranCounter = 0 means no transaction was  
            -- started before the procedure was called.  
            -- The procedure must commit the transaction  
            -- it started.  
            COMMIT TRANSACTION;  
    END TRY  
    BEGIN CATCH  
        -- An error occurred; must determine  
        -- which type of rollback will roll  
        -- back only the work done in the  
        -- procedure.  
        IF @TranCounter = 0  
            -- Transaction started in procedure.  
            -- Roll back complete transaction.  
            ROLLBACK TRANSACTION;  
        ELSE  
            -- Transaction started before procedure  
            -- called, do not roll back modifications  
            -- made before the procedure was called.  
            IF XACT_STATE() <> -1  
                -- If the transaction is still valid, just  
                -- roll back to the savepoint set at the  
                -- start of the stored procedure.  
                ROLLBACK TRANSACTION ProcedureSave;  
                -- If the transaction is uncommitable, a  
                -- rollback to the savepoint is not allowed  
                -- because the savepoint rollback writes to  
                -- the log. Just return to the caller, which  
                -- should roll back the outer transaction.  
  
        -- After the appropriate rollback, echo error  
        -- information to the caller.  
        DECLARE @ErrorMessage NVARCHAR(4000);  
        DECLARE @ErrorSeverity INT;  
        DECLARE @ErrorState INT;  
  
        SELECT @ErrorMessage = ERROR_MESSAGE();  
        SELECT @ErrorSeverity = ERROR_SEVERITY();  
        SELECT @ErrorState = ERROR_STATE();  
  
        RAISERROR (@ErrorMessage, -- Message text.  
                   @ErrorSeverity, -- Severity.  
                   @ErrorState -- State.  
                   );  
    END CATCH  
GO  









