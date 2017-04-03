# Ejercicio de Xamarin & Microsoft Cognitive Services

## Requisitos

Para poder realizar este ejercicio es necesario contar o crear una cuenta de Microsoft Cognitive Services puedes generarla directamente [aqui](https://www.microsoft.com/cognitive-services/)

## Subscribirte a Cognitive Services

En la pagina principal vamos a la seccion "My Account"

<img src="Imagenes/1.png"/>

Nota: Recibiremos un correo de confimarcion de nuestra cuenta.

Una vez dentro de nuestro perfil podremos activar los servicios que requiramos dando click en el boton de añadir.

<img src="Imagenes/2.png"/>

En la siguiente pantalla podremos activar tantos servicios como necesitemos ademas de poder cada uno de los detalles y costos.

<img src="Imagenes/3.png"/>

Despues de aceptar los terminos regresamos a la pagina de perfil en el cual podremos encontrar las credenciales para acceder a la API que eligimos.

<img src="Imagenes/4.png"/>

## Aplicacion Xamarin

Primero añadiremos el codigo de implementacion el cual puedes encontrar [aqui](https://github.com/Microsoft/Cognitive-Face-Windows/tree/master/ClientLibrary)

### Paso 1 Inicializar el cliente y crear un id de grupo

```

FaceServiceClient = new FaceServiceClient("yourToken");
_personGroupId = Guid.NewGuid().ToString();

```

### Paso 2 Crear agrupación

```

await FaceServiceClient.CreatePersonGroupAsync(_personGroupId, "MVPs In Mexico");

```

### Paso 3 Definir personas del grupo

```

var p = await FaceServiceClient.CreatePersonAsync(_personGroupId, "Nombre");

```

### Paso 4 Añadir caras a las personas

```

await FaceServiceClient.AddPersonFaceAsync(_personGroupId, p.PersonId, "Url de Foto");

```

### Paso 4 Entrenar

```

await FaceServiceClient.TrainPersonGroupAsync(_personGroupId);
//Verificar Status de entrenamiento
TrainingStatus trainingStatus = null;
while (true)
{
      trainingStatus = await FaceServiceClient.GetPersonGroupTrainingStatusAsync(_personGroupId);
			if (trainingStatus.Status != Status.Running)
			{
					break;
			}
			await Task.Delay(1000);
}

```

### Paso 5 Buscar

```

//Detectamos rostros en nuestra imagen (Stream faceData)
var faces = await FaceServiceClient.DetectAsync(faceData);

//Convertimos nuestro resultado en un arreglo 
var faceIds = faces.Select(face => face.FaceId).ToArray();

//Identificamos los Ids encontrados
var results = await FaceServiceClient.IdentifyAsync(_personGroupId, faceIds);

//Seleccionamos el primer Id de los candidatos
var result = results[0].Candidates[0].PersonId;

//Obtenemos los datos del candidato
var person = await FaceServiceClient.GetPersonAsync(_personGroupId, result);

```
