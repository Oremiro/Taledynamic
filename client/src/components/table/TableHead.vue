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
        <div style="display: flex; align-items: center; justify-content: center; padding: 0.8rem .5rem;">
          <n-form-item
            v-if="isCreatingInputShown"
            :show-label="false"
            :show-feedback="false"
            :rule="headerNameRule"
          >
          <n-input
            ref="creatingInput"
            v-model:value="newHeaderName"
            placeholder=""
            autosize
            size="small"
            :maxlength="50"
            style="max-width: 10rem;"
            @blur="hideCreatingInput"
            @keyup.enter="createColumn"
          />
          </n-form-item>
          <n-button
            v-else
            circle
            size="small"
            secondary
            @click="showCreatingInput"
          >
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
import { ref, reactive, computed, nextTick } from "vue";
import { FormItemRule, NInput, useThemeVars } from "naive-ui";
import { TableHeader } from "@/models/table";
import TableHeaderVue from "@/components/table/TableHeader.vue";
import { DraggableList } from "@/components/table/draggable";
import { AddIcon } from "@/components/icons";
import { stringValidator } from "@/helpers";

const props = defineProps<{
  data: TableHeader[];
}>();

const emit = defineEmits<{
  (e: "swap", indexFirst: number, indexSecond: number): void;
  (e: "create", name: string): void;
}>();

const tableHeaders = computed<TableHeader[]>(() => props.data);

const draggableList = reactive<DraggableList>(new DraggableList("headers"));
function dropCallback(index: number, itemIndex: number) {
  emit("swap", index, itemIndex);
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

function createColumn() {
  if (isHeaderNameValid.value) {
    emit("create", newHeaderName.value);
  }
  hideCreatingInput();
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
