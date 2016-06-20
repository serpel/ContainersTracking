use Containers
go

declare @fechaInicio datetime = '2016-06-13 12:00'
declare @fechaFinal datetime = '2016-06-19 16:00'

--movimientos contenedores por compania
select row_number() over ( partition by cd.Name order by c.InsertedAt asc) Numero,
       cd.Name as Compania,
       r.Name as Region,
       case when c.[Type] = 1 then 'Entrada' else 'Salida' end Tipo,
	   case when c.ContainerStatus = 1 then 'Lleno' else 'Vacio' end [EstadoContenedor],
       c.ContainerNumber as NoContenedor, 
       c.ContainerLicensePlate as PlacaCab, 
	   c.ContainerLabel as Marchamo, 
	   c.InsertedAt Fecha
  from dbo.ContainerTrackings c
       inner join dbo.Companies co on co.CompanyId = c.CompanyOriginId
	   inner join dbo.Companies cd on cd.CompanyId = c.CompanyDestinationId
       inner join dbo.Drivers d on c.DriverId = d.DriverId
	   inner join dbo.Regions r on r.RegionId = cd.RegionId
 where cd.IsActive = 1
   and c.InsertedAt between @fechaInicio and @fechaFinal
 order by c.InsertedAt asc
 --where c.[Type] = 1 -- Container este adentro
  --round(isnull(datediff(HH, c.InsertedAt, GETDATE())/24.0,0),2) Estadia

go
-- Inventario de contenedores 
select row_number() over ( partition by co.Name order by c.InsertedAt asc) Numero,
       cast(r.Name as varchar) as Zona,
       cast(co.Name as varchar) as Compania,
       c.ContainerNumber as NoContenedor,
       (case when c.ContainerStatus = 1 then 'Lleno' else 'Vacio' end ) as EstadoContenedor,
       c.ContainerLicensePlate as Placa,
	   c.ContainerLabel as Marchamo,
	   c.InsertedAt as Fecha,
	   round(isnull(datediff(HH, c.InsertedAt, GETDATE())/24.0,0),2) Estadia
  from (
		select a.ContainerNumber,
			  (select top 1 c.ContainerTrackingId
				 from dbo.ContainerTrackings c 
				where c.ContainerNumber = a.ContainerNumber
				order by c.InsertedAt desc) ContainerTrackingId
		from (
			 select distinct c.ContainerNumber  
			   from dbo.ContainerTrackings c
		   ) a
		) cc
		inner join dbo.ContainerTrackings c on c.ContainerTrackingId = cc.ContainerTrackingId
		inner join dbo.Companies co on co.CompanyId = c.CompanyDestinationId
		inner join dbo.Regions r on r.RegionId = co.RegionId
  where c.[Type] = 1
    and co.IsActive = 1
  order by c.InsertedAt asc


declare @fechaInicio datetime = '2016-06-13 12:00'
declare @fechaFinal datetime = '2016-06-19 16:00'

-- Reporte de Cierre
select cd.Name as Compania,
       r.Name as Region,
       case when c.[Type] = 1 then 'Entrada' else 'Salida' end Tipo,
	   case when c.ContainerStatus = 1 then 'Lleno' else 'Vacio' end [EstadoContenedor],
       c.ContainerNumber as NoContenedor, 
       c.ContainerLicensePlate as PlacaCab, 
	   c.ContainerLabel as Marchamo,
	   c.InsertedAt Ingresado,
	   c.InsertedBy IngresadoPor,
	   c.UpdatedAt Actualizado,
	   c.UpdatedBy ActualizadoPor
  from dbo.ContainerTrackings c
	   inner join dbo.Companies cd on cd.CompanyId = c.CompanyDestinationId
	   inner join dbo.Regions r on r.RegionId = cd.RegionId
 where cd.IsActive = 1
   and c.InsertedAt between @fechaInicio and @fechaFinal
 order by c.InsertedAt asc