<template>
  <thead>
    <transition-group name="list-complete" tag="tr">
      <th
        v-for="(header, index) of tableHeaders"
        :key="header.id"
        class="list-complete-item draggable"
        :class="{
          start: index === dragStartIndex,
          over: index === dragOverIndex
        }"
        :draggable="true"
        @dragstart.self="dragStartHandler($event, index)"
        @dragend="dragEndHandler"
        @drop="dropHandler($event, index)"
        @dragover.prevent
        @dragenter.prevent="dragEnterHandler($event, index)"
      >
        <table-header-vue :name="header.name" />
      </th>
    </transition-group>
  </thead>
</template>

<script setup lang="ts">
import { ref, computed } from "vue";
import { useThemeVars } from "naive-ui";
import { TableHeader } from "@/models/table";
import TableHeaderVue from "@/components/table/TableHeader.vue";

const props = defineProps<{
  data: TableHeader[]
}>();

const emit = defineEmits<{
  (e: "swap", indexFirst: number, indexSecond: number): void
}>();

const tableHeaders = computed(() => props.data)

const dragStartIndex = ref<number>();
const dragOverIndex = ref<number>();

function dragStartHandler(e: DragEvent, index: number) {
  dragStartIndex.value = index;
  if (e.dataTransfer) {
    e.dataTransfer.dropEffect = "copy";
    e.dataTransfer.effectAllowed = "copy";
    e.dataTransfer.setData("itemIndex", index.toString());
  }
}

function dragEnterHandler(e: DragEvent, index: number) {
  if (e.dataTransfer) {
    const dataItemIndex: string = e.dataTransfer?.getData("itemIndex");
    const itemIndex: number = parseInt(dataItemIndex);
    if (!isNaN(itemIndex)) {
      if (index !== itemIndex) {
        dragOverIndex.value = index;
      } else {
        dragOverIndex.value = undefined;
      }
    }
  }
}

function dragEndHandler() {
  dragStartIndex.value = undefined;
  dragOverIndex.value = undefined;
}

function dropHandler(e: DragEvent, index: number) {
  if (e.dataTransfer) {
    const dataItemIndex: string = e.dataTransfer?.getData("itemIndex");
    const itemIndex: number = parseInt(dataItemIndex);
    if (!isNaN(itemIndex)) {
      if (index !== itemIndex) {
        emit("swap", index, itemIndex);
      }
    }
  }
}

const themeVars = useThemeVars();
</script>

<style scoped lang="scss">
.draggable {
  cursor: move;
}
.draggable.start {
  opacity: 0.8;
}
.draggable.over {
  border-bottom: 1px solid v-bind("themeVars.primaryColor");
}

[draggable] {
  user-select: none;
}

.list-complete-item {
  transition: all 0.8s ease;
}

.list-complete-enter-from,
.list-complete-leave-to {
  opacity: 0;
  transform: translateY(30px);
}

.list-complete-leave-active {
  position: absolute;
}
</style>
