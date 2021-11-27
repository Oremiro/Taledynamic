<template>
  <tbody>
    <transition-group name="list-complete">
      <tr
        v-for="(row, index) in tableRows"
        :key="row.id"
        :draggable="
          index !== tableRows.length - 1 &&
          store.getters['table/editableRowIndex'] !== index
        "
        class="list-complete-item"
        @dragstart="draggableList.dragStartHandler($event, index)"
        @dragend="draggableList.dragEndHandler($event)"
        @drop.prevent="draggableList.dropHandler($event, index, dropCallback)"
        v-on="{ dragover: index !== tableRows.length - 1 ? (event: Event) => event.preventDefault() : null }"
        @dragenter.prevent="draggableList.dragEnterHandler($event, index)"
      >
        <table-row-vue
          :index="index"
          :last="index === tableRows.length - 1"
          :draggable-status="
            index === draggableList.dragStartIndex
              ? 'start'
              : index === draggableList.dragEnterIndex
              ? 'enter'
              : undefined
          "
          :data="row.cells"
          @update="rowUpdateHandler(index)"
        />
      </tr>
    </transition-group>
  </tbody>
</template>

<script setup lang="ts">
import { reactive, computed } from "vue";
import { TableRow } from "@/models/table";
import { DraggableList } from "@/components/table/draggable";
import TableRowVue from "@/components/table/TableRow.vue";
import { useStore } from "@/store";

const store = useStore();
const tableRows = computed<TableRow[]>(() => store.getters["table/rows"]);

const draggableList = reactive<DraggableList>(new DraggableList("rows"));
async function dropCallback(index: number, itemIndex: number) {
  if (index === tableRows.value.length - 1) return;
  try {
    await store.dispatch("table/swapRows", {
      indexFirst: index,
      indexSecond: itemIndex
    });
  } catch (error) {
    if (error instanceof Error) {
      console.log(error);
    }
  }
}

async function rowUpdateHandler(rowIndex: number) {
  if (rowIndex === tableRows.value.length - 1) {
    await store.dispatch("table/addRow");
  }
}
</script>

<style scoped lang="scss">
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
