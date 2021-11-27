<template>
  <div
    style="
      display: flex;
      align-items: center;
      justify-content: space-between;
      padding: 0.5rem 0.8rem;
      gap: 0.5rem;
    "
  >
    <div style="display: flex; gap: 0.5rem; align-items: center">
      <n-input
        v-model:value="headerName"
        autosize
        size="small"
        placeholder=""
        :style="{
          'background-color': themeVars.tableHeaderColor,
          'max-width': '10rem'
        }"
        @mouseenter="store.commit('table/setEditableHeaderIndex', { index: index })"
        @mouseleave="store.commit('table/clearEditableHeaderIndex')"
      />
      <n-popconfirm v-if="store.getters['table/headers'].length > 1" v-model:show="isConfirmShown">
        <template #icon>
          <n-icon :color="themeVars.errorColor">
            <error-circle-icon />
          </n-icon>
        </template>
        <template #action>
          <n-button ghost type="error" size="small" @click="emit('delete')">
            Да
          </n-button>
          <n-button ghost size="small" @click="isConfirmShown = false">
            Нет
          </n-button>
        </template>
        <template #trigger>
          <dynamically-typed-button
            style="padding: 0 0.3rem"
            type="error"
            size="small"
            quaternary
          >
            <n-icon size="1.1rem">
              <delete-icon />
            </n-icon>
          </dynamically-typed-button>
        </template>
        <div>Удалить колонку?</div>
      </n-popconfirm>
    </div>
    <n-button style="padding: 0 0.3rem" quaternary size="small">
      <n-icon size="1.1rem">
        <arrow-sort-icon></arrow-sort-icon>
      </n-icon>
    </n-button>
  </div>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { useThemeVars, NPopconfirm } from "naive-ui";
import { ArrowSortIcon, DeleteIcon, ErrorCircleIcon } from "@/components/icons";
import DynamicallyTypedButton from "@/components/DynamicallyTypedButton.vue";
import { useStore } from "@/store";

const props = defineProps<{
  name: string;
  index: number;
}>();

const emit = defineEmits<{
  (e: "delete"): void;
}>();

const headerName = ref<string>(props.name);
const isConfirmShown = ref<boolean>(false);
const store = useStore();

const themeVars = useThemeVars();
</script>
