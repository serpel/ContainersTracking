use Containers
go

create view DailyCloseView
AS

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
   --and c.InsertedAt between @fechaInicio and @fechaFinal
 --order by c.InsertedAt asc