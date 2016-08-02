use Containers
go

select * 
  from [dbo].[ContainerTrackings] c 
 where c.ContainerTrackingId = '3193'

update [dbo].[ContainerTrackings]
   set InsertedAt = '2016-07-25 15:07'
 where ContainerTrackingId = '3179'

SELECT 
C.*,
co.Name Compania
 FROM [dbo].[ContainerTrackings] c
inner join [dbo].[Companies] co on co.CompanyId = c.CompanyDestinationId
where c.CompanyDestinationId = '249'

select * from dbo.[Companies] c where c.code = '806'

select row_number() over ( partition by c.Compania order by c.FechaCreacion asc) [No],
       c.*
 from Containers.dbo.ContainersInventoryView c
where c.Movimiento = 'Contenedor'
order by c.Zona asc,
         c.Compania asc,
         c.FechaCreacion asc

--select distinct Movimiento from dbo.ContainersInventoryView 
alter view dbo.ContainersInventoryView 
AS
select cast(r.Name as varchar) as Zona,
       cast(co.Name as varchar) as Compania,
	   case when c.[TrackingType] = 0 then 'Contenedor' 
	        when c.[TrackingType] = 1 then 'Camion'
		else 'Rastra' end Movimiento,
       c.ContainerNumber as Numero,
       (case when c.ContainerStatus = 1 then 'Lleno' else 'Vacio' end ) as Estado,
       c.ContainerLicensePlate as Placa,
	   c.ContainerLabel as Marchamo,
	   c.InsertedAt FechaCreacion,
	   c.InsertedBy UsuarioCreador,
	   c.UpdatedAt FechaEdicion,
	   c.UpdatedBy UsuarioEditor,
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
		inner join dbo.Regions ra on ra.RegionId = c.GateId
  where c.[Type] = 1 --entrada
    and co.IsActive = 1
 -- order by c.InsertedAt asc