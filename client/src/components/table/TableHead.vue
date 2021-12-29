<template>
  <thead>
    <tr>
      <th scope="col" />
      <th
        v-for="(header, index) of tableHeaders"
        :key="header.id"
        scope="col"
        class="draggable"
        :class="{
          start: index === draggableList.dragStartIndex,
          'enter--left':
            index === draggableList.dragEnterIndex &&
            draggableList.dragStartIndex !== undefined &&
            index < draggableList.dragStartIndex,
          'enter--right':
            index === draggableList.dragEnterIndex &&
            draggableList.dragStartIndex !== undefined &&
            index > draggableList.dragStartIndex
        }"
        :draggable="index !== store.getters['table/editableHeaderIndex'] && !store.getters['table/isImmutable']"
        @dragstart="draggableList.dragStartHandler($event, index)"
        @dragend="draggableList.dragEndHandler"
        @drop.prevent="draggableList.dropHandler($event, index, dropCallback)"
        @dragover.prevent
        @dragenter.prevent="draggableList.dragEnterHandler($event, index)"
      >
        <table-header-vue :index="index" @delete="deleteColumn(index)" />
      </th>
      <th>
        <div v-if="!store.getters['table/isImmutable']" style="padding: 0 .6rem;">
          <n-form-item v-if="isCreatingInputShown" :show-label="false" :show-feedback="false" :rule="headerNameRule">
            <n-input
              ref="creatingInput"
              v-model:value="newHeaderName"
              placeholder=""
              autosize
              size="small"
              :maxlength="100"
              style="max-width: 10rem"
              @blur="hideCreatingInput"
              @keyup.enter="addColumn"
            />
          </n-form-item>
          <n-button v-else style="padding: 0 0.3rem" size="small" ghost @click="showCreatingInput">
            <n-icon size="1.2rem">
              <add-icon />
            </n-icon>
          </n-button>
        </div>
      </th>
    </tr>
  </thead>
</template>

<script setup lang="ts">
import { ref, reactive, computed, nextTick } from "vue";
import { FormItemRule, NInput, useThemeVars } from "naive-ui";
import { TableDataType, TableHeader } from "@/models/table";
import TableHeaderVue from "@/components/table/TableHeader.vue";
import { DraggableList } from "@/components/table/draggable";
import { AddIcon } from "@/components/icons";
import { stringValidator } from "@/helpers";
import { useStore } from "@/store";

const store = useStore();
const tableHeaders = computed<TableHeader[]>(() => store.getters["table/headers"]);

const draggableList = reactive<DraggableList>(new DraggableList("headers"));
async function dropCallback(index: number, itemIndex: number) {
  try {
    await store.dispatch("table/moveColumn", {
      oldIndex: itemIndex,
      newIndex: index
    });
  } catch (error) {
    if (error instanceof Error) {
      console.log(error);
    }
  }
}

const newHeaderName = ref<string>("");
const isHeaderNameValid = ref<boolean>(false);
const headerNameRule: FormItemRule = {
  trigger: "input",
  asyncValidator: async (): Promise<void> => {
    isHeaderNameValid.value = false;
    try {
      await stringValidator(newHeaderName.value);
      isHeaderNameValid.value = true;
    } catch (error) {
      if (error instanceof Error) {
        throw error;
      }
    }
  }
};
const creatingInput = ref<InstanceType<typeof NInput>>();
const isCreatingInputShown = ref<boolean>(false);
async function showCreatingInput() {
  isCreatingInputShown.value = true;
  await nextTick();
  creatingInput.value?.focus();
}
function hideCreatingInput() {
  isCreatingInputShown.value = false;
  newHeaderName.value = "";
  isHeaderNameValid.value = false;
}

async function addColumn() {
  if (isHeaderNameValid.value) {
    await store.dispatch("table/addColumn", {
      name: newHeaderName.value,
      type: TableDataType.Text
    });
  }
  hideCreatingInput();
}

async function deleteColumn(index: number) {
  try {
    await store.dispatch("table/deleteColumn", { index: index });
  } catch (error) {
    if (error instanceof Error) {
      console.log(error);
    }
  }
}

const themeVars = useThemeVars();
</script>

<style scoped lang="scss">
@import "@/components/table/style.scss";
th {
  padding: 0;
}
.draggable.enter--left {
  border-left: 1px solid v-bind("themeVars.primaryColor");
}
.draggable.enter--right {
  border-right: 1px solid v-bind("themeVars.primaryColor");
}
</style>
