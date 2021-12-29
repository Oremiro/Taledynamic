<template>
  <n-config-provider :theme-overrides="themeVars.tableColor === '#fff' ? lightThemeOverrides : darkThemeOverrides">
    <n-input
      v-if="type === 0"
      ref="textInputRef"
      v-model:value="cellDataText"
      placeholder=""
      :readonly="true"
      @focus="showTextArea"
    />
    <n-table
      v-if="isTextAreaShown"
      style="position: fixed; z-index: 1000"
      :style="{
        width: textInputRef?.$el.getBoundingClientRect().width + 'px',
        top: textInputRef?.$el.getBoundingClientRect().top + 'px'
      }"
    >
      <n-input
        ref="textAreaInputRef"
        v-model:value="cellDataText"
        type="textarea"
        :autosize="{
          minRows: 3,
          maxRows: 6
        }"
        placeholder=""
        @update:value="dataUpdateHandler"
        @blur="hideTextArea"
        @mouseenter="$emit('mouseEnterCell')"
        @mouseleave="$emit('mouseLeaveCell')"
      />
    </n-table>
    <n-input-number
      v-if="type === 1"
      v-model:value="cellDataNumber"
      :show-button="false"
      placeholder=""
      :readonly="disabled"
      @update:value="dataUpdateHandler"
      @mouseenter="$emit('mouseEnterCell')"
      @mouseleave="$emit('mouseLeaveCell')"
    />
    <n-date-picker
      v-if="type === 2"
      v-model:value="cellDataDate"
      type="datetime"
      :first-day-of-week="0"
      placeholder=""
      :readonly="disabled"
      format="dd.MM.yyyy HH:mm"
      @update:value="dataUpdateHandler"
      @mouseenter="$emit('mouseEnterCell')"
      @mouseleave="$emit('mouseLeaveCell')"
    />
    <table-cell-image v-if="type === 3" :value="cellDataImage" @update="dataUpdateHandler" />
    <table-cell-file v-if="type === 4" :value="cellDataFile" @update="dataUpdateHandler" />
  </n-config-provider>
</template>

<script setup lang="ts">
import { ref, reactive, nextTick, watch } from "vue";
import { NInputNumber, NDatePicker, GlobalThemeOverrides, useThemeVars, NInput, NTable } from "naive-ui";
import { isTableDataFile, TableData, TableDataFile, TableDataType } from "@/models/table";
import TableCellImage from "@/components/table/TableCellImage.vue";
import TableCellFile from "@/components/table/TableCellFile.vue";

const emit = defineEmits<{
  (e: "update", index: number, data: TableData): void;
  (e: "mouseEnterCell"): void;
  (e: "mouseLeaveCell"): void;
}>();

const props = defineProps<{
  data: TableData;
  index: number;
  type: TableDataType;
  disabled: boolean;
}>();

const cellDataText = ref<string | null>(null);
const cellDataNumber = ref<number | null>(null);
const cellDataDate = ref<number | null>(null);
const cellDataImage = ref<string | null>(null);
const cellDataFile = ref<TableDataFile | null>(null);

watch(
  () => props.data,
  () => {
    cellDataText.value = props.type === TableDataType.Text && typeof props.data === "string" ? props.data : null;
    cellDataNumber.value = props.type === TableDataType.Number && typeof props.data === "number" ? props.data : null;
    cellDataDate.value = props.type === TableDataType.Date && props.data instanceof Date ? props.data.getTime() : null;
    cellDataImage.value = props.type === TableDataType.Image && typeof props.data === "string" ? props.data : null;
    cellDataFile.value = props.type === TableDataType.File && isTableDataFile(props.data) ? props.data : null;
  },
  { immediate: true }
);

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

function dataUpdateHandler(data: string | number | TableDataFile | null): void {
  let normalizedData: TableData;
  if (props.type === TableDataType.Date && typeof data === "number") {
    normalizedData = new Date(data);
  } else {
    normalizedData = data;
  }
  emit("update", props.index, normalizedData);
}

const isTextAreaShown = ref<boolean>(false);
const textAreaInputRef = ref<InstanceType<typeof NInput>>();
const textInputRef = ref<InstanceType<typeof NInput>>();

async function showTextArea(): Promise<void> {
  if (props.disabled) return;
  isTextAreaShown.value = true;
  await nextTick();
  textAreaInputRef.value?.focus();
}

function hideTextArea(): void {
  isTextAreaShown.value = false;
}

const themeVars = useThemeVars();
</script>
