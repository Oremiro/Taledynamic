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
        @update:value="dataUpdateHandler(cellDataText)"
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
      @update:value="dataUpdateHandler(cellDataNumber)"
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
      @update:value="dataUpdateHandler(cellDataDate)"
      @mouseenter="$emit('mouseEnterCell')"
      @mouseleave="$emit('mouseLeaveCell')"
    />
    <table-cell-image v-if="type === 3" />
  </n-config-provider>
</template>

<script setup lang="ts">
import { ref, reactive, nextTick, watch } from "vue";
import {
  NInputNumber,
  NDatePicker,
  GlobalThemeOverrides,
  useThemeVars,
  NInput,
  NTable,
  useMessage,
} from "naive-ui";
import { TableData, TableDataType } from "@/models/table";
import TableCellImage from "@/components/table/TableCellImage.vue";

const emit = defineEmits<{
  (e: "update", index: number, data: TableData): void;
  (e: "mouseEnterCell"): void;
  (e: "mouseLeaveCell"): void;
}>();

const props = defineProps<{
  data: TableData;
  index: number;
  type: TableDataType;
}>();

const cellDataText = ref<string | null>(null);
const cellDataNumber = ref<number | null>(null);
const cellDataDate = ref<number | null>(null);
const cellDataImage = ref<string | null>("data:image/jpeg;base64,R0lGODdhAQABAPAAAP8AAAAAACwAAAAAAQABAAACAkQBADs=");
const cellDataFile = ref<string | null>(null);

const message = useMessage();


watch(
  () => props.data,
  () => {
    cellDataText.value = props.type === TableDataType.Text && typeof props.data === "string" ? props.data : null;
    cellDataNumber.value = props.type === TableDataType.Number && typeof props.data === "number" ? props.data : null;
    cellDataDate.value = props.type === TableDataType.Date && props.data instanceof Date ? props.data.getTime() : null;
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

function dataUpdateHandler(data: string | number | null): void {
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

async function showTextArea() {
  isTextAreaShown.value = true;
  await nextTick();
  textAreaInputRef.value?.focus();
}

function hideTextArea() {
  isTextAreaShown.value = false;
}

const themeVars = useThemeVars();
</script>
