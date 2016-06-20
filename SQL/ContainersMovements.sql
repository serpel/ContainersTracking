use Containers
go

create view ContainerMovementsView
as

--declare @fechaInicio datetime = '2016-06-13 12:00'
--declare @fechaFinal datetime = '2016-06-19 16:00'

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
   --and c.InsertedAt between @fechaInicio and @fechaFinal
-- order by c.InsertedAt asc