<template>
  <component
    :is="tag"
    ref="draggableRef"
    :style="style"
    @mousedown.left="mouseDownHandler"
    @mouseup="mouseUpHandler"
  >
    <slot />
  </component>
</template>

<script setup lang="ts">
import { ref, reactive, CSSProperties } from "vue";

withDefaults(defineProps<{ tag?: string }>(), {
  tag: "div"
});

const draggableRef = ref();
const isDragging = ref<boolean>(false);

const shiftX = ref<number>(0);
const shiftY = ref<number>(0);

const style = reactive<CSSProperties>({
  position: undefined,
  left: undefined,
  top: undefined,
  zIndex: undefined,
  userSelect: "none",
  width: undefined,
  height: undefined,
  cursor: "move"
});

function moveAt(pageX: number, pageY: number) {
  style.left = pageX - shiftX.value + "px";
  style.top = pageY - shiftY.value + "px";
}

async function mouseDownHandler(event: MouseEvent) {
  isDragging.value = true;
  style.width = draggableRef.value.offsetWidth + 'px';
  style.height = draggableRef.value.offsetHeight + 'px';
  shiftX.value =
    event.clientX - draggableRef.value.getBoundingClientRect().left;
  shiftY.value = event.clientY - draggableRef.value.getBoundingClientRect().top;
  style.position = "fixed";
  style.zIndex = 1000;

  moveAt(event.pageX, event.pageY);
  addEventListener("mousemove", mouseMoveHandler);
}

function mouseUpHandler() {
  isDragging.value = false;
  style.position = undefined;
  style.zIndex = undefined;
  style.left = undefined;
  style.top = undefined;
  style.width = undefined;
  style.height = undefined;
  removeEventListener("mousemove", mouseMoveHandler);
}

function mouseMoveHandler(event: MouseEvent) {
  if (isDragging.value) {
    moveAt(event.pageX, event.pageY);
  }
}
</script>
