<template>
  <n-button
    v-if="!isInputShown"
    ghost
    type="primary"
    @click="showTableNameInput"
  >
    <n-icon size="1.2rem">
      <add-icon />
    </n-icon>
  </n-button>
  <n-input
    v-else
    ref="tableNameInput"
    v-model:value="tableName"
    autosize
    :maxlength="30"
    :show-count="true"
    placeholder=""
    @blur="hideTableNameInput"
    @keyup.enter="createTable"
  />
</template>

<script setup lang="ts">
import { ref, nextTick } from "vue";
import AddIcon from "@/components/icons/AddIcon.vue";
import { NInput } from "naive-ui";

const tableNameInput = ref<InstanceType<typeof NInput>>();
const isInputShown = ref<boolean>(false);
const tableName = ref<string>("Table #1");

async function showTableNameInput() {
  isInputShown.value = true;
  await nextTick();
  tableNameInput.value?.focus();
}

function hideTableNameInput() {
  isInputShown.value = false;
}

async function createTable() {
  console.log('TODO')
  tableNameInput.value?.blur();
}
</script>
