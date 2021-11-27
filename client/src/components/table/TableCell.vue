<template>
  <n-config-provider
    :theme-overrides="
      themeVars.tableColor === '#fff' ? lightThemeOverrides : darkThemeOverrides
    "
  >
    <n-input
      v-if="type === 0"
      v-model:value="cellDataText"
      placeholder=""
      @update:value="emit('update', cellDataText)"
      @mouseenter="$emit('mouseEnterCell')"
      @mouseleave="$emit('mouseLeaveCell')"
    />
    <n-input-number
      v-if="type === 1"
      v-model:value="cellDataNumber"
      :show-button="false"
      placeholder=""
      @update:value="emit('update', cellDataNumber)"
      @mouseenter="$emit('mouseEnterCell')"
      @mouseleave="$emit('mouseLeaveCell')"
    />
    <n-date-picker
      v-if="type === 2"
      v-model:value="cellDataDate"
      :actions="[]"
      :first-day-of-week="0"
      placeholder=""
      format="dd.MM.yyyy"
      @update:value="emit('update', cellDataDate)"
      @mouseenter="$emit('mouseEnterCell')"
      @mouseleave="$emit('mouseLeaveCell')"
    />
  </n-config-provider>
</template>

<script setup lang="ts">
import { ref, reactive } from "vue";
import {
  NInputNumber,
  NDatePicker,
  GlobalThemeOverrides,
  useThemeVars
} from "naive-ui";
import { TableDataType } from "@/models/table";

const emit = defineEmits<{
  (e: "update", data?: string | number | Date): void;
  (e: "mouseEnterCell"): void;
  (e: "mouseLeaveCell"): void;
}>();

const props = defineProps<{
  data?: string | number | Date;
  type: TableDataType;
}>();

const cellDataText = ref<string>(
  props.type === TableDataType.Text && typeof props.data === "string"
    ? props.data
    : ""
);
const cellDataNumber = ref<number | undefined>(
  props.type === TableDataType.Number && typeof props.data === "number"
    ? props.data
    : undefined
);
const cellDataDate = ref<number | undefined>(
  props.type === TableDataType.Date && props.data instanceof Date
    ? props.data.getTime()
    : undefined
);
// const cellDataAttachment = ref<string>();

const darkThemeOverrides = reactive<GlobalThemeOverrides>({
  Input: {
    color: "rgb(24, 24, 28)"
  }
});

const lightThemeOverrides = reactive<GlobalThemeOverrides>({
  Input: {
    color: "#fff",
    border: "none"
  }
});

const themeVars = useThemeVars();
</script>
