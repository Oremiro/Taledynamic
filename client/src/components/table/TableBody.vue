<template>
  <transition-group name="list-complete" tag="tbody">
    <transition-group
      v-for="(row, index) in tableRows"
      :key="row.id"
      :draggable="index !== tableRows.length - 1"
      tag="tr"
      name="list-complete"
      class="list-complete-item"
      @dragstart="draggableList.dragStartHandler($event, index)"
      @dragend="draggableList.dragEndHandler($event)"
      @drop.prevent="draggableList.dropHandler($event, index, dropCallback)"
      @dragover.prevent
      @dragenter.prevent="draggableList.dragEnterHandler($event, index)"
    >
      <table-row-vue
        :index="index"
        :last="index === tableRows.length - 1"
        :draggable="
          index === draggableList.dragStartIndex
            ? 'start'
            : index === draggableList.dragEnterIndex
            ? 'enter'
            : undefined
        "
        :data="row.cells"
        @update="rowUpdateHandler(index)"
      />
    </transition-group>
  </transition-group>
</template>

<script setup lang="ts">
import { reactive, computed } from "vue";
import { useThemeVars } from "naive-ui";
import { TableRow } from "@/models/table";
import { DraggableList } from "@/components/table/draggable";
import TableRowVue from "@/components/table/TableRow.vue";
import { useStore } from "@/store";

const store = useStore();
const tableRows = computed<TableRow[]>(() => store.getters["table/rows"]);

const draggableList = reactive<DraggableList>(new DraggableList("rows"));
async function dropCallback(index: number, itemIndex: number) {
  try {
    await store.dispatch("table/swapRows", { index, itemIndex });
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
