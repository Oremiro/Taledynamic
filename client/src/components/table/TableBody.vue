<template>
  <transition-group name="list-complete" tag="tbody">
    <tr v-for="(row, index) in tableRows" :key="row.id">
      <td
        :draggable="true"
        style="width: 0; padding: 0;"
        class="list-complete-item draggable"
        :class="{
          start: index === draggableList.dragStartIndex,
          over: index === draggableList.dragEnterIndex
        }"
        @dragstart="draggableList.dragStartHandler($event, index)"
        @dragend="draggableList.dragEndHandler"
        @drop="draggableList.dropHandler($event, index, dropCallback)"
        @dragover.prevent
        @mousemove="draggableList.dragOverHandler"
        @dragenter.prevent="draggableList.dragEnterHandler($event, index)"
      ></td>
      <td v-for="cell in row.cells" :key="cell.id" style="padding: 0">
        <table-cell-vue :data="cell.data" :type="cell.type" />
      </td>
    </tr>
    <tr :key="0">
      <td></td>
      <td v-for="cell of rowLength" :key="cell" style="padding: 0">
        <table-cell-vue :type="0" />
      </td>
    </tr>
  </transition-group>
</template>

<script setup lang="ts">
import { reactive, computed } from "vue";
import TableCellVue from "@/components/table/TableCell.vue";
import { TableRow } from "@/models/table";
import { useThemeVars } from "naive-ui";
import { DraggableList } from "./draggable";

const props = defineProps<{
  data: TableRow[];
  rowLength: number;
}>();

const emit = defineEmits<{
  (e: "swap", indexFirst: number, indexSecond: number): void;
}>();

const tableRows = computed<TableRow[]>(() => props.data);

const draggableList = reactive<DraggableList>(new DraggableList());
function dropCallback(index: number, itemIndex: number) {
  console.log('123')
  emit("swap", index, itemIndex);
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
  border-left: 1px solid v-bind("themeVars.primaryColor");
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
