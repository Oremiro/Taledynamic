<template>
  <div class="container" style="padding: 2rem">
    <n-table :single-line="false">
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
      <tbody>
        <tr v-for="row in tableRows" :key="row.id">
          <td v-for="cell in row.cells" :key="cell.id" style="padding: 0">
            <table-cell-vue :data="cell.data" :type="cell.type" />
          </td>
        </tr>
        <tr>
          <td
            v-for="cell of tableHeaders.length"
            :key="cell"
            style="padding: 0"
          >
            <table-cell-vue :type="0" />
          </td>
        </tr>
      </tbody>
    </n-table>
  </div>
</template>

<script setup lang="ts">
import { reactive, computed, ref } from "vue";
import { NTable, useThemeVars } from "naive-ui";
import {
  TableRow,
  TableHeader,
  TableDataType,
  TableCell
} from "@/models/table";
import TableCellVue from "@/components/table/TableCell.vue";
import TableHeaderVue from "@/components/table/TableHeader.vue";

const props = defineProps<{
  workspaceId: string;
  tableId: string;
}>();

const workspaceId = computed<number>(() => parseInt(props.workspaceId));
const tableId = computed<number>(() => parseInt(props.tableId));

const tableHeaders = reactive<Array<TableHeader>>([
  new TableHeader("Товар", TableDataType.Text),
  new TableHeader("Стоимость", TableDataType.Number),
  new TableHeader("Количество", TableDataType.Number),
  new TableHeader("Дата производства", TableDataType.Date)
]);
const tableRows = reactive<Array<TableRow>>([
  new TableRow([
    new TableCell("Пылесос", TableDataType.Text),
    new TableCell(20000, TableDataType.Number),
    new TableCell(3, TableDataType.Number),
    new TableCell(new Date(Date.now()), TableDataType.Date)
  ])
]);

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
        [tableHeaders[index], tableHeaders[itemIndex]] = [
          tableHeaders[itemIndex],
          tableHeaders[index]
        ];
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
