# 🎮 ¡Actividades de las Semanas 3 y 4!

¡Hola! 👋 En este repositorio podrás encontrar el trabajo que realicé para las actividades de las semanas 3 y 4.

## 🌿 Navegación por Ramas (Branches)

¿Deseas ver cuál fue mi progreso en cada semana? ¡Es muy fácil!
- **Semana 3:** Cambia a la rama `DesarroS3`.
- **Semana 4:** Mantente en la rama `AvancesS4` (esta es la rama por defecto).

---

## ✨ Características Destacadas

### 🕹️ Control del Player
El sistema del jugador está dividido en dos partes principales:
1. Los controles de movimiento del personaje.
2. El sistema que representa su vida y el recolector de ítems.

**💡 Mecánica de control:**
- Presiona **`Shift Izquierdo`** para que el personaje se detenga.
- Presiona **`Control Izquierdo`** para que el recolector/vida se detenga de forma independiente.

### 🎬 Demostración Visual
> *A continuación, un vistazo a las mecánicas en acción:*

<div align="center">
  <img src="URL_DE_TU_PRIMER_GIF.gif" width="400" alt="Demostración del Player">
  &nbsp;&nbsp;&nbsp;&nbsp;
  <img src="URL_DE_TU_SEGUNDO_GIF.gif" width="400" alt="Demostración del Recolector">
</div>

---

## 📜 ¿Qué hay de los Scripts?

Para hacer que todo esto funcione, se crearon un total de **11 scripts**. Aquí te comparto las implementaciones técnicas nuevas que más me gustaron:

* 🗂️ **Organización en el Inspector:** Implementación del atributo `[Header("Titulo")]` para crear encabezados y ordenar de manera elegante nuestros valores en el Inspector de Unity.
* 🟢 **Gizmos:** Uso del método `OnDrawGizmosSelected()` para dibujar y visualizar el radio de colisión del `Colector` directamente en la escena.
* 🧮 **Cálculos Matemáticos:** Uso de métodos de la clase `Mathf` para definir límites, realizar redondeos y aplicar valores infinitos negativos para asegurar que nuestros métodos se activen correctamente con cualquier valor superior.
* 🖥️ **Interfaz de Usuario (UI):** Uso de componentes `TextMeshProUGUI` para mostrar la vida, experiencia (XP) y el puntaje (Score) del Player en pantalla, recolectando estos datos de forma limpia desde el `BaseData` del Player.

> ¡Hay más implementaciones bajo el capó, pero estas son las que más disfruté incluir en el proyecto! :D

