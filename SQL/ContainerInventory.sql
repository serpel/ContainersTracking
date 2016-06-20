use Containers
go

create view dbo.ContainersInventoryView 
AS
select row_number() over ( partition by co.Name order by c.InsertedAt asc) Numero,
       r.Name as Zona,
       co.Name as Compania,
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
 -- order by c.InsertedAt asc
