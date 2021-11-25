<template>
  <thead>
    <transition-group name="list-complete" tag="tr">
      <th :key="0" scope="col" class="list-complete-item" />
      <th
        v-for="(header, index) of tableHeaders"
        :key="header.id"
        scope="col"
        class="list-complete-item draggable"
        :class="{
          start: index === draggableList.dragStartIndex,
          enter: index === draggableList.dragEnterIndex
        }"
        :draggable="true"
        @dragstart="draggableList.dragStartHandler($event, index)"
        @dragend="draggableList.dragEndHandler"
        @drop.prevent="draggableList.dropHandler($event, index, dropCallback)"
        @dragover.prevent
        @dragenter.prevent="draggableList.dragEnterHandler($event, index)"
      >
        <table-header-vue :name="header.name" />
      </th>
      <th :key="1">
        <div style="display: flex; align-items: center; justify-content: center;">
          <n-button circle size="small" secondary>
            <n-icon size="1.2rem">
              <add-icon />
            </n-icon>
          </n-button>
        </div>
      </th>
    </transition-group>
  </thead>
</template>

<script setup lang="ts">
import { reactive, computed } from "vue";
import { useThemeVars } from "naive-ui";
import { TableHeader } from "@/models/table";
import TableHeaderVue from "@/components/table/TableHeader.vue";
import { DraggableList } from "@/components/table/draggable";
import { AddIcon } from "@/components/icons";

const props = defineProps<{
  data: TableHeader[];
}>();

const emit = defineEmits<{
  (e: "swap", indexFirst: number, indexSecond: number): void;
}>();

const tableHeaders = computed<TableHeader[]>(() => props.data);

const draggableList = reactive<DraggableList>(new DraggableList("headers"));
function dropCallback(index: number, itemIndex: number) {
  emit("swap", index, itemIndex);
}

const themeVars = useThemeVars();
</script>

<style scoped lang="scss">
th {
  padding: 0;
}
.draggable {
  cursor: move;
}
.draggable.start {
  opacity: 0.8;
}
.draggable.enter {
  border-bottom: 1px solid v-bind("themeVars.primaryColor");
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
