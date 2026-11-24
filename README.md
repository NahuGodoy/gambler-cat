# 🎰 Gambler Cat

**Gambler Cat** es un videojuego de plataformas 3D desarrollado por **Victory Entertainment** con Unity. Inspirado en los clásicos juegos de plataformas de acción y aventura, combina desafíos de precisión, combate, exploración y recolección con una estética colorida y una ambientación temática de casino.

Acompañá a nuestro protagonista felino en una aventura donde cada ficha cuenta. Superá obstáculos, derrotá enemigos, conseguí recompensas y avanzá por distintos niveles para recuperar tus ganancias y saldar una deuda.

El juego ofrece una experiencia dinámica y rejugable, con diferentes niveles de dificultad, mejoras desbloqueables, desafíos contrarreloj y progresión persistente.

---

## 🎮 Características principales

- **Plataformas 3D:** movimiento fluido, carrera, salto variable y doble salto desbloqueable.
- **Combate:** ataques cuerpo a cuerpo y ataques por pisada.
- **Enemigos variados:** enemigos terrestres, estáticos, móviles y voladores.
- **Obstáculos y desafíos:** trampas, proyectiles y plataformas que ponen a prueba tus reflejos.
- **Power-ups:** habilidades temporales de invencibilidad, velocidad e imán de fichas.
- **Sistema de checkpoints:** guardá tu progreso durante los niveles y reaparecé desde la última posición segura.
- **Coleccionables:** encontrá fichas, objetos especiales y secretos escondidos en los niveles.
- **Sistema de progresión:** acumulá fichas y utilizalas en la tienda para conseguir mejoras.
- **Dificultades:** elegí entre fácil, normal y difícil, cada una con sus propios desafíos y recompensas.
- **Contrarreloj:** completá los niveles en el menor tiempo posible y superá tus propios récords.
- **Sistema de logros:** desbloqueá logros a medida que explorás y progresás.
- **Multijugador local:** compartí la experiencia con amigos mediante pantalla dividida.
- **Personalización:** configurá los controles y las opciones de juego según tus preferencias.
- **Eventos especiales:** disfrutá de eventos aleatorios y de la ruleta de la fortuna en el lobby.

## 🌎 Escenarios y modos de juego

### Lobby

El centro principal de la aventura. Desde aquí podrás acceder a los diferentes niveles, consultar tu progreso, visitar la tienda y participar en la ruleta de la fortuna.

### Tutorial

Aprendé las mecánicas fundamentales del juego mediante una introducción interactiva al movimiento, los saltos, los ataques, las interacciones y la recolección de fichas.

### Nivel 1

Comenzá tu aventura en un recorrido lleno de plataformas, enemigos, obstáculos y coleccionables. Disponible en tres niveles de dificultad.

### Nivel 2

Continuá la aventura enfrentándote a nuevos desafíos. Al igual que el primer nivel, cuenta con diferentes dificultades y registros de tiempo independientes.

## 🪙 Progresión y economía

Las fichas son la moneda principal de Gambler Cat. Podrás conseguirlas explorando los niveles y recolectando objetos durante tus partidas.

Las fichas obtenidas se acreditan al completar cada nivel y pueden utilizarse en la tienda para adquirir mejoras y beneficios.

Entre las posibilidades de progresión se encuentran:

- Desbloqueo de habilidades, como el doble salto.
- Compra de mejoras y power-ups.
- Recolección de objetos especiales.
- Desbloqueo de logros.
- Progresión hacia el objetivo de saldar la deuda.

### Dificultades

Cada nivel ofrece tres dificultades:

| Dificultad | Multiplicador de fichas |
|---|---:|
| Fácil | 1× |
| Normal | 2× |
| Difícil | 3× |

La dificultad también puede modificar la configuración de los checkpoints y los desafíos presentes en cada nivel.

## 🏆 Récords y logros

El juego registra el mejor tiempo obtenido en cada nivel y dificultad, permitiendo comparar tus resultados y mejorar tus marcas.

Además, cuenta con un sistema de logros que registra diferentes hitos y actividades realizadas durante la aventura.

## 💾 Sistema de guardado

Gambler Cat cuenta con tres ranuras de guardado independientes.

Cada partida conserva información sobre:

- Estado del tutorial y niveles desbloqueados.
- Fichas acumuladas.
- Coleccionables obtenidos.
- Logros desbloqueados.
- Récords de tiempo por nivel y dificultad.
- Mejoras y compras realizadas.

Los datos se almacenan localmente mediante archivos JSON, permitiendo conservar el progreso entre sesiones.

## 👥 Multijugador local

Gambler Cat incorpora soporte para partidas multijugador locales mediante el sistema de entrada de Unity.

Los jugadores pueden participar utilizando teclado o mandos compatibles, con cámaras independientes y visualización mediante pantalla dividida.

## ⌨️ Controles

Los controles pueden personalizarse desde el menú de opciones.

| Acción | Teclado y ratón | Mando |
|---|---|---|
| Moverse | `W` `A` `S` `D` o flechas | Stick izquierdo |
| Controlar cámara | Ratón | Stick derecho |
| Saltar | `Espacio` | Botón Sur |
| Correr | `Shift Izquierdo` | Pulsar stick izquierdo |
| Atacar | Clic izquierdo o `Enter` | Botón Oeste |
| Interactuar | `E` | Botón Norte |
| Usar power-up | `Q` | Botón Oeste |
| Rotar power-up | `R` | Shoulder derecho |
| Pausar | `Escape` | Botón Este |

*Los controles pueden modificarse desde el menú de reasignación de teclas.*

---

## 📥 Descarga e instalación

Podés descargar Gambler Cat desde la sección **Releases** de este repositorio.

Seleccioná el archivo `.zip` correspondiente a tu sistema operativo:

| Sistema operativo | Archivo ejecutable |
|---|---|
| Windows | `GamblerCat.exe` |
| Linux | `GamblerCat.x86_64` |

### Windows

1. Descargá el archivo `.zip` de Windows.
2. Extraé todo su contenido en una carpeta.
3. Ejecutá `GamblerCat.exe`.

### Linux

1. Descargá el archivo `.zip` de Linux.
2. Extraé todo su contenido en una carpeta.
3. Abrí una terminal dentro de la carpeta extraída.
4. Concedé permisos de ejecución al archivo:

   ```bash
   chmod +x GamblerCat.x86_64
   ```

5. Ejecutá el juego:

   ```bash
   ./GamblerCat.x86_64
   ```

**Importante:** conservá todos los archivos y carpetas incluidos en el `.zip`. El ejecutable necesita sus recursos asociados para funcionar correctamente.

---

## 🛠️ Tecnologías utilizadas

Gambler Cat está desarrollado utilizando las siguientes tecnologías y herramientas:

| Tecnología | Utilización |
|---|---|
| Unity 6 (`6000.5.8f1`) | Motor de desarrollo |
| C# | Lenguaje de programación |
| Universal Render Pipeline | Renderizado gráfico |
| Unity Input System | Gestión de entradas |
| Cinemachine | Sistema de cámaras |
| TextMesh Pro | Renderizado de texto |
| UGUI | Interfaces de usuario |
| AI Navigation | Navegación de enemigos |
| Timeline | Secuencias y eventos |
| JsonUtility | Serialización y persistencia de datos |

## 🧩 Estructura del proyecto

El proyecto está organizado dentro de la carpeta `Assets/`, siguiendo una estructura modular para facilitar el desarrollo y mantenimiento.

```text
Assets/
├── Scenes/              Escenas principales y de prueba
├── Scripts/             Lógica y sistemas del juego
│   ├── Achievements/    Sistema de logros
│   ├── Ataque/          Sistemas de combate y daño
│   ├── MenuPrincipal/   Lógica del menú principal
│   ├── OpcionesLogica/  Configuración del juego
│   ├── Tienda/          Sistema de tienda y mejoras
│   └── UI/              Interfaces y elementos visuales
├── Prefabs/             Personajes, enemigos y objetos
├── Resources/           Recursos cargables dinámicamente
├── Music/               Música y efectos de sonido
├── TextMesh Pro/        Recursos tipográficos
└── InputSystem_Actions  Configuración de entradas
```

### Sistemas principales

Entre los principales componentes y sistemas implementados se encuentran:

- `PlayerMovement`: movimiento, carrera, saltos y desplazamiento relativo a la cámara.
- `PlayerHealth`: gestión de vida, daño, invulnerabilidad y reaparición.
- `PlayerStomp`: sistema de ataque por pisada.
- `PlayerPowerUps`: administración de habilidades temporales.
- `SaveManager` y `SaveData`: gestión de partidas y persistencia.
- `Checkpoint`: gestión de puntos de control.
- `LobbyController`: navegación y funcionalidades del lobby.
- `ScenarioOneDifficultyManager` y `ScenarioTwoDifficultyManager`: configuración de dificultades.
- `GameManager`: administración del estado de las partidas.
- `AchievementSystem`: validación y desbloqueo de logros.
- `SplitScreenCamera`: configuración de cámaras para multijugador local.

---

## 💻 Desarrollo

Para abrir y ejecutar el proyecto desde el editor de Unity:

1. Instalar [Unity Hub](https://unity.com/download).
2. Instalar Unity `6000.5.8f1`.
3. Clonar o descargar este repositorio.
4. Agregar la carpeta raíz del proyecto desde Unity Hub.
5. Abrir el proyecto y esperar a que Unity importe los recursos y dependencias.
6. Abrir la escena `Assets/Scenes/MenuPrincipal.unity`.
7. Presionar **Play** para ejecutar el juego desde el editor.

Las escenas utilizadas por el juego se encuentran registradas en `ProjectSettings/EditorBuildSettings.asset`.

### Dependencias

Los paquetes y dependencias del proyecto se encuentran declarados en `Packages/manifest.json`.

Entre ellos se incluyen:

- Universal Render Pipeline.
- Cinemachine.
- Unity Input System.
- TextMesh Pro.
- UGUI.
- AI Navigation.
- Timeline.
- Visual Scripting.
- Unity Test Framework.

---

## 🎨 Créditos

**Gambler Cat** es un videojuego creado y desarrollado por **Victory Entertainment**.

Un proyecto realizado en equipo, combinando programación, diseño de niveles, diseño de interfaces, pruebas y desarrollo de videojuegos.

**Victory Entertainment — Game Development.**

---

*© 2026 Victory Entertainment. Todos los derechos reservados.*
