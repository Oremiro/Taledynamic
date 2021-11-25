<template>
  <div
    style="
      display: flex;
      align-items: center;
      justify-content: space-between;
      padding: 0.8rem;
    "
  >
    <div style="display: flex; gap: 0.5rem; align-items: center">
      <n-input
        v-model:value="headerName"
        autosize
        size="small"
        placeholder=""
        :style="{ 'background-color': themeVars.tableHeaderColor, 'max-width': '10rem' }"
      />
      <n-popconfirm v-if="!isLast" v-model:show="isConfirmShown">
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
          <dynamically-typed-button type="error" size="small" quaternary @click.stop>
            <n-icon size="1.1rem">
              <delete-icon />
            </n-icon>
          </dynamically-typed-button>
        </template>
        <div>Удалить колонку?</div>
      </n-popconfirm>
    </div>
    <n-button quaternary size="small">
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

const props = defineProps<{
  name: string;
  isLast: boolean;
}>();

const emit = defineEmits<{
  (e: "delete"): void
}>();

const headerName = ref<string>(props.name);

const isConfirmShown = ref<boolean>(false);

const themeVars = useThemeVars();
</script>
