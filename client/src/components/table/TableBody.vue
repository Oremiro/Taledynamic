<template>
  <transition-group name="list-complete" tag="tbody">
    <transition-group
      v-for="(row, index) in tableRows"
      :key="row.id"
      :draggable="true"
      tag="tr"
      name="list-complete"
      class="list-complete-item"
      @dragstart="draggableList.dragStartHandler($event, index)"
      @dragend="draggableList.dragEndHandler($event)"
      @drop.prevent="draggableList.dropHandler($event, index, dropCallback)"
      @dragover.prevent
      @dragenter.prevent="draggableList.dragEnterHandler($event, index)"
    >
      <th
        :key="0"
        scope="row"
        class="list-complete-item draggable"
        :class="{
          start: index === draggableList.dragStartIndex,
          enter: index === draggableList.dragEnterIndex
        }"
      ></th>
      <td
        v-for="cell in row.cells"
        :key="cell.id"
        class="list-complete-item"
        style="padding: 0"
        :draggable="true"
        @dragstart.prevent
      >
        <table-cell-vue :data="cell.data" :type="cell.type" />
      </td>
      <td :key="1"></td>
    </transition-group>
  </transition-group>
</template>

<script setup lang="ts">
import { reactive, computed } from "vue";
import TableCellVue from "@/components/table/TableCell.vue";
import { TableRow } from "@/models/table";
import { useThemeVars } from "naive-ui";
import { DraggableList } from "@/components/table/draggable";

const props = defineProps<{
  data: TableRow[];
  rowLength: number;
}>();

const emit = defineEmits<{
  (e: "swap", indexFirst: number, indexSecond: number): void;
}>();

const tableRows = computed<TableRow[]>(() => props.data);

const draggableList = reactive<DraggableList>(new DraggableList("rows"));
function dropCallback(index: number, itemIndex: number) {
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
.draggable.enter {
  border-left: 1px solid v-bind("themeVars.primaryColor");
}

.list-complete-item {
  transition: all 0.5s ease;
}

.list-complete-enter-from,
.list-complete-leave-to {
  opacity: 0;
  transform: translateX(30px);
}

.list-complete-leave-active {
  position: absolute;
}
</style>
