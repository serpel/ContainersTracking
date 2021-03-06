/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP 1000 [EmpCod]
      ,[EmpNom]
      ,[Emp1erNom]
      ,[Emp2doNom]
      ,[Emp1erApe]
      ,[Emp2doApe]
      ,[EmpRTN]
      ,[EmpMarcContr]
      ,[EmpSiNo]
      ,[EmpLocExt]
      ,[EmpDirec]
      ,[OpcionID]
      ,[EmpHabili]
      ,[EmpUsuaria]
      ,[EmpPaiCod]
      ,[EmpCiuCod]
  FROM [DUAS].[dbo].[EMPRESAS]

  order by EmpNom asc

  use DUAS
  GO

  select * 
    from [dbo].[EMPRESAS] e
	where e.EmpCod = 605
    order by e.EmpNom asc

  select * from [dbo].[MOTORISTAS]
  
  select distinct rtrim(ltrim(e.MovExtGuardia)) as Nombre
   from [dbo].[MOVTOEXTCONTE] e
   order by Nombre asc


   select * from dbo.usuarios
  select * 
    from [dbo].[MOVTOINTCONTE]



  --select * 
  --  from [dbo].[MOVTOEXTCONTE] m
  -- where m.MovExtFechsal = '1753-01-01 00:00:00.000'

  select top 1 *
    from [dbo].[MOVTOEXTCONTE] m
   where m.MovExtTieneSalida <> 'S'

   use DUAS
   go

  select m.ConteNumero as ContainerNumber,
		 m.MovExtContLleno as  [ContainerStatus],
		 m.MovExtDuaNum as DUA,
		 m.MovExtMarcNum as Marchamo,
		 m.MovExtBlNum as BLNUM,
		 m.UsrUser as UserIn,
		 m.MovExtConduc as Driver,
		 m.MovExtCedula as DriverId,
		 m.MovExtNoPlaca as LicencePlate,
		 m.MovExtGuardia as SecuritySupervisor,
		 e.EmpNom as Origin,
		 --e1.EmpNom as Destination,
		 DATEADD(ss, datepart(ss, m.MovExtHoraES), DATEADD(mi,datepart(mi, m.MovExtHoraES),DATEADD(hh,datepart(hh, m.MovExtHoraES),m.MovExtFech))) as InsertedAt,
		 getdate() as UpdatedAt     
    from [dbo].[MOVTOEXTCONTE] m
	     inner join [dbo].[EMPRESAS] e on e.EmpCod = m.EmpCodDirije
		 --inner join [dbo].[EMPRESAS] e1 on e.EmpCod = m.UbicaCod
   where m.MovExtTieneSalida <> 'S'



