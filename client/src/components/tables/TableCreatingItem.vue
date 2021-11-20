<template>
  <transition name="fade" mode="out-in" @enter="focusTableNameInput">
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
    <n-form-item
      v-else
      ref="tableNameFormItem"
      :show-label="false"
      :show-feedback="false"
      :rule="tableNameRule"
    >
      <n-input-group>
        <n-input
          ref="tableNameInput"
          v-model:value="tableName"
          autosize
          :maxlength="30"
          :show-count="true"
          placeholder=""
          @blur="hideTableNameInput"
          @keyup.enter="createTable"
        />
        <n-button
          v-if="isTableNameValid"
          attr-type="submit"
          style="padding: 0.6rem"
          @click="createTable"
        >
          <n-icon size="1.2rem">
            <checkmark-icon />
          </n-icon>
        </n-button>
      </n-input-group>
    </n-form-item>
  </transition>
</template>

<script setup lang="ts">
import axios from "axios";
import { ref, computed } from "vue";
import {
  FormItemRule,
  NInputGroup,
  NFormItem,
  NInput,
  useMessage
} from "naive-ui";
import AddIcon from "@/components/icons/AddIcon.vue";
import { TableApi } from "@/helpers/api/table";
import { useStore } from "@/store";
import { stringValidator } from "@/helpers";
import CheckmarkIcon from "@/components/icons/CheckmarkIcon.vue";

const props = defineProps<{
  workspaceId: number;
  tablesCount: number;
}>();

const tableNameInput = ref<InstanceType<typeof NInput>>();
const isInputShown = ref<boolean>(false);
const defaultTableName = computed<string>(() => `Table #${ props.tablesCount + 1 }`);
const tableName = ref<string>(defaultTableName.value);

const tableNameFormItem = ref<InstanceType<typeof NFormItem>>();
const isTableNameValid = ref<boolean>(false);
const tableNameRule: FormItemRule = {
  trigger: "input",
  asyncValidator: async (): Promise<void> => {
    isTableNameValid.value = false;
    try {
      await stringValidator(tableName.value);
      isTableNameValid.value = true;
    } catch (error) {
      if (error instanceof Error) {
        throw error;
      }
    }
  }
};

function focusTableNameInput(): void {
  if (isInputShown.value) {
    tableNameInput.value?.focus();
  }
}

function showTableNameInput(): void {
  isInputShown.value = true;
}

function hideTableNameInput(): void {
  isInputShown.value = false;
  tableName.value = defaultTableName.value;
}

const store = useStore();
const message = useMessage();

async function createTable(): Promise<void> {
  try {
    await tableNameFormItem.value?.validate();
    try {
      const { data } = await TableApi.create(
        {
          name: tableName.value,
          workspaceId: props.workspaceId
        },
        store.getters["user/accessToken"]
      );
      console.log(data.table);
    } catch (error) {
      // TODO: 401 Handler
      if (axios.isAxiosError(error)) {
        console.log(error.message);
      }
    }
    tableNameInput.value?.blur();
  } catch (error) {
    message.error("Некорректные данные");
  }
}
</script>
