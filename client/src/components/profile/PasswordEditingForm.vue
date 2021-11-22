<template>
  <n-form ref="formRef" :model="formData" :rules="rules">
    <n-form-item first label="Новый пароль" path="newPassword.value">
      <n-input
        v-model:value="formData.newPassword.value"
        type="password"
        show-password-on="click"
        placeholder=""
        :loading="isNewPwdValidationPending"
        @input="handlePasswordInput"
      >
        <template v-if="!formData.newPassword.isValid" #prefix>
          <question-tooltip>
            Пароль должен содержать минимум 8 символов, заглавную букву,
            строчную букву, цифру и специальный символ.
          </question-tooltip>
        </template>
      </n-input>
    </n-form-item>
    <n-form-item
      ref="confirmedPasswordRef"
      first
      label="Повторите новый пароль"
      path="confirmedPassword.value"
    >
      <n-input
        v-model:value="formData.confirmedPassword.value"
        type="password"
        show-password-on="click"
        placeholder=""
        :loading="isConfirmedPwdValidationPending"
      />
    </n-form-item>
    <n-collapse-transition
      :show="formData.newPassword.isValid && formData.confirmedPassword.isValid"
    >
      <n-form-item first label="Текущий пароль" path="currentPassword.value">
        <n-input
          v-model:value="formData.currentPassword.value"
          type="password"
          show-password-on="click"
          placeholder=""
        />
      </n-form-item>
      <delayed-button
        ref="submitButtonRef"
        attr-type="submit"
        type="success"
        ghost
        style="margin-right: 1rem"
        :loading="isSubmitButtonLoading"
        :disabled="
          !formData.currentPassword.value ||
          !formData.newPassword.isValid ||
          !formData.confirmedPassword.isValid ||
          isSubmitButtonLoading ||
          isNewPwdValidationPending ||
          isConfirmedPwdValidationPending
        "
        @click="submitForm"
      >
        Сохранить
      </delayed-button>
      <n-button ghost type="error" @click="undoChanges"> Отменить </n-button>
    </n-collapse-transition>
  </n-form>
</template>

<script setup lang="ts">
import { reactive, ref } from "vue";
import { FormRules, NForm, NFormItem, useMessage } from "naive-ui";
import { useStore } from "@/store";
import { debounce, passwordRegex } from "@/helpers";
import { PasswordEditFormData } from "@/models";
import QuestionTooltip from "@/components/QuestionTooltip.vue";
import DelayedButton from "@/components/DelayedButton.vue";

const formRef = ref<InstanceType<typeof NForm>>();
const formData = reactive<PasswordEditFormData>({
  currentPassword: {
    value: "",
    isValid: false
  },
  newPassword: {
    value: "",
    isValid: false
  },
  confirmedPassword: {
    value: "",
    isValid: false
  }
});

const isNewPwdValidationPending = ref<boolean>(false);
const isConfirmedPwdValidationPending = ref<boolean>(false);
const rules: FormRules = {
  currentPassword: {
    value: [
      {
        required: true,
        message: "Пожалуйста, введите текущий пароль",
        trigger: "input"
      }
    ]
  },
  newPassword: {
    value: [
      {
        required: true,
        asyncValidator: debounce(
          (rule, value) => {
            isNewPwdValidationPending.value = false;
            if (!passwordRegex.test(value)) {
              formData.newPassword.isValid = false;
              throw new Error("Введите корректный сложный пароль");
            } else {
              formData.newPassword.isValid = true;
            }
          },
          700,
          {
            immediateFunc: () => {
              isNewPwdValidationPending.value = true;
            }
          }
        ),
        trigger: "input"
      }
    ]
  },
  confirmedPassword: {
    value: [
      {
        required: true,
        asyncValidator: debounce(
          (rule, value) => {
            isConfirmedPwdValidationPending.value = false;
            if (value !== formData.newPassword.value) {
              formData.confirmedPassword.isValid = false;
              throw new Error("Пароли не совпадают");
            } else {
              formData.confirmedPassword.isValid = true;
            }
          },
          700,
          {
            immediateFunc: () => {
              isConfirmedPwdValidationPending.value = true;
            }
          }
        ),
        trigger: ["input", "password-input"]
      }
    ]
  }
};
const store = useStore();
const message = useMessage();
const confirmedPasswordRef = ref<InstanceType<typeof NFormItem>>();
const submitButtonRef = ref<InstanceType<typeof DelayedButton>>();
const isSubmitButtonLoading = ref<boolean>(false);

function handlePasswordInput(): void {
  if (formData.confirmedPassword.value != "") {
    confirmedPasswordRef.value
      ?.validate({ trigger: "password-input" })
      .catch(() => true);
  }
}

function undoChanges(): void {
  formData.currentPassword.value = "";
  formData.newPassword.value = "";
  formData.confirmedPassword.value = "";
  formData.newPassword.isValid = false;
  formData.confirmedPassword.isValid = false;
  formRef.value?.restoreValidation();
}

function submitForm(): void {
  isSubmitButtonLoading.value = true;
  formRef.value?.validate(async (errors) => {
    if (!errors) {
      try {
        await store.dispatch("user/updatePassword", {
          currentPassword: formData.currentPassword.value,
          newPassword: formData.newPassword.value,
          confirmedNewPassword: formData.confirmedPassword.value
        });
        undoChanges();
        message.success("Вы успешно изменили пароль");
      } catch (error) {
        if (error instanceof Error) {
          message.error(error.message);
        }
      } finally {
        isSubmitButtonLoading.value = false;
        // @ts-expect-error: vue-next #4397
        submitButtonRef.value?.holdDisabled();
      }
    } else {
      message.error("Данные не являются корректными");
      isSubmitButtonLoading.value = false;
      // @ts-expect-error: vue-next #4397
      submitButtonRef.value?.holdDisabled();
    }
  });
}
</script>
