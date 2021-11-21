<template>
  <n-form-item
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
        :show-count="!loading"
        :maxlength="30"
        placeholder=""
        :loading="loading"
        :disabled="loading"
        @blur="emit('blur', $event)"
        @keyup.enter="validate"
      />
      <n-button
        v-if="isTableNameValid && !loading"
        attr-type="submit"
        style="padding: 0.6rem"
        @click="validate"
      >
        <n-icon size="1.2rem">
          <checkmark-icon />
        </n-icon>
      </n-button>
    </n-input-group>
  </n-form-item>
</template>

<script setup lang="ts">
import { ref, onMounted } from "vue";
import { NInput, NFormItem, FormItemRule, useMessage } from "naive-ui";
import { stringValidator } from "@/helpers";
import { CheckmarkIcon } from "@/components/icons";

const props = defineProps<{
  defaultName: string;
  loading?: boolean;
}>();

const emit = defineEmits<{
  (e: "blur", event?: FocusEvent): void;
  (e: "valid", name: string): void;
}>();

const tableName = ref<string>(props.defaultName);

const tableNameFormItem = ref<InstanceType<typeof NFormItem>>();
const isTableNameValid = ref<boolean>(true);
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

const message = useMessage();

async function validate(): Promise<void> {
  try {
    await tableNameFormItem.value?.validate();
    emit("valid", tableName.value);
  } catch (error) {
    message.error("Некорректные данные");
  }
}

const tableNameInput = ref<InstanceType<typeof NInput>>();
onMounted(() => {
  tableNameInput.value?.focus();
});
</script>
