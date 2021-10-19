--linea de comandos para entity framework
--Scaffold-DBContext "Server=localhost;Database=netbanking;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -o Modelo -f

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
Abreviado varchar(3) not null unique,
Simbolo varchar(5) not null,
)
go

create table MonedaCambios(
MonedaCambioID int identity constraint PK_MonedaCambios primary key,
MonedaIDdesde int not null constraint FK_MonedaCambios_MonedaIDdesde foreign key references Monedas(MonedaID),
MonedaIDhacia int not null constraint FK_MonedaCambios_MonedaIDhasta foreign key references Monedas(MonedaID),
Valor numeric(18,15) not null default 0.0,
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
AliasCuenta varchar(100),
NumeroCuenta varchar(100) not null,
MontoDisponible numeric(38,2) not null default 0.0,
MontoTrancito numeric(38,2) not null default 0.0
)
go

create table Retiros(
RetiroID int identity constraint PK_Retiros primary key,
CuentaID int not null constraint FK_Retiros_CuentaID foreign key references Cuentas(CuentaID),
UsuarioID int not null constraint FK_Retiros_UsuarioID foreign key references Usuarios(UsuarioID),
EstadoID int not null constraint FK_Retiros_EstadoID foreign key references EstadoTRD(EstadoID),
Monto numeric(35,2) not null default 0.0,
Fecha datetime,
Detalles varchar(max)
)
go

create table Depositos(
DepositoID int identity constraint PK_Depositos primary key,
CuentaID int not null constraint FK_Depositos_CuentaID foreign key references Cuentas(CuentaID),
UsuarioID int not null constraint FK_Depositos_UsuarioID foreign key references Usuarios(UsuarioID),
EstadoID int not null constraint FK_Depositos_EstadoID foreign key references EstadoTRD(EstadoID),
Monto numeric(35,2) not null default 0.0,
Fecha datetime,
Detalles varchar(max)
)
go

create table Transacciones(
TransaccionID int identity constraint PK_Transacciones primary key,
RetiroID int not null constraint FK_Transacciones_RetiroID foreign key references Retiros(RetiroID),
DepositoID int not null constraint FK_Transacciones_DepositoID foreign key references Depositos(DepositoID),
EstadoID int not null constraint FK_Transacciones_EstadoID foreign key references EstadoTRD(EstadoID),
CostoTransferencia numeric(18,15) not null default 0.0,
Fecha datetime,
MonedaCambioID int constraint FK_Transacciones_MonedaCambioID foreign key references MonedaCambios(MonedaCambioID),
Detalles varchar(max)
)
go


alter table Cuentas add constraint CHK_MondoDiponible_minimo check (MontoDisponible >= 0)
go
alter table Cuentas add constraint UQ_NumeroCuenta unique (NumeroCuenta)
go
alter table MonedaCambios add constraint UQ_MonedaIDdesde_MonedaIDhacia unique (MonedaIDdesde,MonedaIDhacia)
go
insert into EstadoTRD values('Procesada'),('Cancelada'),('En Espera'),('Error')
insert into [dbo].[Monedas] values ('PESO DOMINICANO', 'RD$', 'DOP'),('DOLAR ESTADOUNIDENSE', 'US$', 'USD'),('EURO', '�', 'EUR')
insert into [dbo].[MonedaCambios] values (1,2,57.00,'Para la venta de 1 Dolar es 57 peso'),(2,1,0.0178571428571429,'Para la compra de 1 Dolar es 56 peso')
										,(1,3,68,'Para la venta de 1 Euro es 68 peso'),(3,1,0.015873015873,'Para la compra de 1 Euro es 63 peso')
										,(2,3,1.16,'Para la venta de 1 Dolar es 1.16 Euro'),(3,2,0.87719298245614,'Para la compra de 1 Euro es 1.14 peso')
GO  


 
CREATE PROCEDURE Transferir  
	@UsuarioID INT,
    @NumeroCuentaRetiro VARCHAR(100) , 
    @NumeroCuentaDeposito VARCHAR(100), 
	@MontoParaDeposito numeric(35,2),
	@Detalles VARCHAR(MAX) =NULL
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
	DECLARE @Impuesto numeric (18,15) = 0.0015 --0.15% DE IMPUESTO
	DECLARE @EstadoID_TRD INT = 3 --EN ESPERA
	DECLARE @CuentaIDretiro INT, @MonedaIDretiro INT,@MontoDisponibleRetiro numeric(38,2)
	,@CuentaIDdeposito INT, @MonedaIDdeposito INT ,@RetiroID int , @DepositoID int
	,@TransaccionID int ,@MonedaCambioID int 

	
	SELECT @CuentaIDretiro=CuentaID, @MonedaIDretiro=MonedaID,@MontoDisponibleRetiro=MontoDisponible FROM [dbo].[Cuentas] WHERE NumeroCuenta = @NumeroCuentaRetiro
	SELECT @CuentaIDdeposito=CuentaID, @MonedaIDdeposito=MonedaID FROM [dbo].[Cuentas] WHERE NumeroCuenta = @NumeroCuentaDeposito

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
		DECLARE @MontoRetiroSinImpuesto numeric(38,2)
		DECLARE @MontoRetiroConImpuesto numeric(38,2)
		DECLARE @CostoTransferencia NUMERIC(18,15)

		IF @MonedaIDretiro <> @MonedaIDdeposito --or @MonedaCambioID = 0
		BEGIN
			DECLARE @ValorCambio NUMERIC(18,15)
			--SELECT @ValorCambio= Valor FROM [dbo].[MonedaCambios] WHERE MonedaCambioID = @MonedaCambioID
			SELECT @MonedaCambioID=MonedaCambioID, @ValorCambio= Valor FROM [dbo].[MonedaCambios] WHERE MonedaIDdesde = @MonedaIDretiro and MonedaIDhacia = @MonedaIDdeposito
			SET @MontoRetiroSinImpuesto = @MontoParaDeposito * @ValorCambio  
		END
		ELSE
		BEGIN
			SET @MontoRetiroSinImpuesto = @MontoParaDeposito
		END
		
		SET @CostoTransferencia = @MontoRetiroSinImpuesto * @Impuesto
		SET @MontoRetiroConImpuesto =  @MontoRetiroSinImpuesto + @CostoTransferencia 

		IF @MontoDisponibleRetiro >= @MontoRetiroConImpuesto
		BEGIN
			INSERT INTO [dbo].Retiros([UsuarioID],[CuentaID],[EstadoID],[Monto],[Detalles]) VALUES (@UsuarioID, @CuentaIDretiro,@EstadoID_TRD,@CostoTransferencia,'Impuesto Art.12 Ley No.288-04')
			INSERT INTO [dbo].Retiros([UsuarioID],[CuentaID],[EstadoID],[Monto],[Detalles]) VALUES (@UsuarioID, @CuentaIDretiro,@EstadoID_TRD,@MontoRetiroSinImpuesto,@Detalles)
			SET @RetiroID = @@IDENTITY

			INSERT INTO [dbo].Depositos([UsuarioID],[CuentaID],[EstadoID],[Monto],[Detalles]) VALUES (@UsuarioID, @CuentaIDdeposito,@EstadoID_TRD,@MontoParaDeposito,@Detalles)
			SET @DepositoID = @@IDENTITY

			INSERT INTO [dbo].[Transacciones] ([RetiroID],[DepositoID],[EstadoID],[CostoTransferencia],[MonedaCambioID],[Detalles]) VALUES
			(@RetiroID,@DepositoID,@EstadoID_TRD,@CostoTransferencia,@MonedaCambioID,@Detalles)
			SET @TransaccionID = @@IDENTITY

			UPDATE [dbo].[Cuentas] SET MontoDisponible=MontoDisponible-@MontoRetiroConImpuesto WHERE CuentaID = @CuentaIDretiro
			UPDATE [dbo].[Cuentas] SET MontoDisponible=MontoDisponible+@MontoParaDeposito WHERE CuentaID = @CuentaIDdeposito
			
			UPDATE [dbo].[Retiros] SET [EstadoID] = 1 WHERE [RetiroID] = @RetiroID --ESTADO PROCESADA
			UPDATE [dbo].[Depositos] SET [EstadoID] = 1 WHERE [DepositoID] = @DepositoID
			UPDATE [dbo].[Transacciones] SET [EstadoID] = 1 WHERE [TransaccionID] = @TransaccionID

		END
        
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
		RETURN 0 
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
		--RETURN 1
		--PENDIENTE CREAR UN TABLA PARA GUARDAR EL @TRANSACCIONID Y EL ERROR 
        RAISERROR (@ErrorMessage, -- Message text.  
                   @ErrorSeverity, -- Severity.  
                   @ErrorState -- State.  
                   );  
    END CATCH  
GO  










