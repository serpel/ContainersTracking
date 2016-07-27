use Containers
go
select * 
  from dbo.ContainerTrackings c

  select distinct Movimiento from dbo.ContainerMovementsView
--select * from dbo.ContainerMovementsView
alter view ContainerMovementsView
as

--declare @fechaInicio datetime = '2016-06-13 12:00'
--declare @fechaFinal datetime = '2016-06-19 16:00'

--movimientos contenedores por compania
select cast(cd.Name as text) as Compania,
       cast(r.Name as text) as ZonaCompania,
	   cast(rp.Name as text) as Porton,
	   case when c.[TrackingType] = 0 then 'Contenedor' 
	        when c.[TrackingType] = 1 then 'Camion'
			else 'Rastra' end Movimiento,
       case when c.[Type] = 1 then 'Entrada' else 'Salida' end Tipo,
	   case when c.ContainerStatus = 1 then 'Lleno' else 'Vacio' end [EstadoContenedor],
       c.ContainerNumber as NoContenedor, 
       c.ContainerLicensePlate as Placa, 
	   c.ContainerLabel as Marchamo, 
	   c.InsertedAt FechaCreacion,
	   c.InsertedBy UsuarioCreador,
	   c.UpdatedAt FechaEdicion,
	   c.UpdatedBy UsuarioEditor
  from dbo.ContainerTrackings c
       inner join dbo.Companies co on co.CompanyId = c.CompanyOriginId
	   inner join dbo.Companies cd on cd.CompanyId = c.CompanyDestinationId
       inner join dbo.Drivers d on c.DriverId = d.DriverId
	   inner join dbo.Regions r on r.RegionId = cd.RegionId
	   inner join dbo.Regions rp on rp.RegionId = c.GateId
 --where cd.IsActive = 1
   --and c.InsertedAt between @fechaInicio and @fechaFinal
-- order by c.InsertedAt asc