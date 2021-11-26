<template>
  <th
    :key="0"
    scope="row"
    class="list-complete-item"
    :class="{
      draggable: !last,
      start: draggable === 'start',
      enter: draggable === 'enter' && !last
    }"
    style="padding: 0 0.3rem"
  >
    <div style="display: flex; align-items: center; justify-content: center">
      <span v-if="!last">{{ index + 1 }}</span>
      <n-icon v-else size="1rem">
        <add-icon />
      </n-icon>
    </div>
  </th>
  <td
    v-for="cell in data"
    :key="cell.id"
    class="list-complete-item"
    style="padding: 0"
    :draggable="true"
    @dragstart.prevent
  >
    <table-cell-vue
      :data="cell.data"
      :type="cell.type"
      @update="emit('update')"
    />
  </td>
  <td :key="1" style="padding: 0">
    <div
      v-if="!last"
      style="display: flex; align-items: center; justify-content: center"
    >
      <n-popconfirm v-if="!last" v-model:show="isConfirmDeletionShown">
        <template #icon>
          <n-icon :color="themeVars.errorColor">
            <error-circle-icon />
          </n-icon>
        </template>
        <template #action>
          <n-button ghost type="error" size="small" @click="deleteRow">
            Да
          </n-button>
          <n-button ghost size="small" @click="isConfirmDeletionShown = false">
            Нет
          </n-button>
        </template>
        <template #trigger>
          <dynamically-typed-button type="error" size="tiny" ghost @click.stop>
            <n-icon size="1rem">
              <delete-icon />
            </n-icon>
          </dynamically-typed-button>
        </template>
        <div>Удалить строку?</div>
      </n-popconfirm>
    </div>
  </td>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { NPopconfirm, useThemeVars } from "naive-ui";
import { AddIcon, DeleteIcon, ErrorCircleIcon } from "@/components/icons";
import TableCellVue from "@/components/table/TableCell.vue";
import DynamicallyTypedButton from "@/components/DynamicallyTypedButton.vue";
import { TableCell } from "@/models/table";
import { useStore } from "@/store";

const props = defineProps<{
  index: number;
  last: boolean;
  draggable?: "enter" | "start";
  data: TableCell[];
}>();

const emit = defineEmits<{
  (e: "update"): void;
}>();

const store = useStore();

async function deleteRow() {
  await store.dispatch("table/deleteRow", { index: props.index });
}
const isConfirmDeletionShown = ref<boolean>(false);
const themeVars = useThemeVars();
</script>
