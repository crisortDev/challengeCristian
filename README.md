# challengeCristian
Prueba tecnica

INSTRUCCIONES PARA LEVANTAR EL PROYECTO:

- Se debe clonar el repositorio: https://github.com/crisortDev/challengeCristian.git mediante GitHub desktop o consola.
- Comando para ejecutar desde la terminal: git clone https://github.com/tu-usuario/nombre-del-repositorio.git

- Luego de clonar verificar que la configuracion de inicio de visual studio este en: https: Ejecutar el proyecto utilizando el protocolo HTTPS o modo de depuración IIS Express.
- Esta configuracion selecciona un navegador por defecto para arrancar y abrir el endpoint Swagger.


INICIALIZAR BASE DE DATOS:
Para inicializar la base de datos seguir lo siguientes pasos:
- Luego de clonar el proyecto debe debe instalar las dependencias necesarias. Debe ejecutar el siguiente comando en la raíz del proyecto Infraestructura (donde está el archivo .csproj): dotnet restore

- Configuración del Archivo de Conexión a la Base de Datos SQLite

- Debe asegurarse de que el archivo de configuración (appsettings.json) tenga la cadena de conexión correcta para SQLite. En el código proporcionado, la cadena de conexión está configurada de la siguiente manera:

"ConnectionStrings": {
  "Sqlite": "Data Source=challenge.db"
}

A modo de aclarar el proyecto ya se encuenta configurado con el data source correcto,

- Aplicar las Migraciones

Para migrar la base de datos y crear las tablas, debe ejecutar el siguiente comando en la terminal (en la raíz del proyecto):

dotnet ef database update

- Si no tienen instalada la herramienta de dotnet-ef, pueden instalarla con:

dotnet tool install --global dotnet-ef

- Levantar el Proyecto

Ahora puede levantar el proyecto y visualizar en el navegafor el swagger endpoint:

dotnet run

Posteriormente puede realizar las pruebas de los endpoints.

DESCRIPCION DEL PROYECTO:
Este proyecto es una API robusta y bien estructurada que implementa los principios SOLID para garantizar un código modular, 
extensible y fácil de mantener. La inyección de dependencias y Entity Framework aseguran que las dependencias estén bien gestionadas y que las operaciones de base de datos sean eficientes y fáciles de manejar. 
Los eventos automáticos permiten un seguimiento completo de los cambios en las tareas, mientras que la documentación generada con Swagger facilita la interacción con la API. 
Este enfoque asegura que el sistema esté preparado para escalar y adaptarse a nuevas funcionalidades sin comprometer la calidad o la estabilidad del código.


EXPLICACIÓN DE DECISIONES TECNICAS:
Las decisiones técnicas tomadas en este proyecto garantizan una arquitectura limpia, mantenible y escalable, utilizando principios sólidos como la inyección de dependencias, 
el patrón repositorio, event sourcing, y mapeo automático con AutoMapper. El uso de GUIDs como identificadores y 
la integración de un sistema de auditoría mediante logging proporciona un sistema robusto y fácil de mantener, 
lo que permite agregar nuevas funcionalidades y realizar pruebas de manera eficiente.
las decisiones técnicas relacionadas con el registro de tareas, asignación de usuarios, actualización de estados, 
y registro de eventos aseguran que cada operación crítica se registre de manera adecuada, proporcionando visibilidad y trazabilidad.
Los eventos se almacenan para mantener un historial completo de las operaciones realizadas, lo que facilita la auditoría y mejora la fiabilidad del sistema. 
El uso de logs detallados y la implementación de validaciones estrictas garantizan la integridad del sistema y permiten una fácil solución de problemas en caso de errores.
