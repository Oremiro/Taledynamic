<template>
  <n-form ref="formRef" :rules="rules" :model="formData">
    <n-form-item first label="Email" path="email.value">
      <n-auto-complete 
      :options="options" 
      v-model:value="formData.email.value" 
      #default="{ handleInput, handleBlur, handleFocus, value }">
        <n-input placeholder="" @input="handleInput" @focus="handleFocus" @blur="handleBlur" :value="value">
          <template v-if="!formData.email.isValid" #prefix>
            <question-tooltip text="Email может содержать только буквы латинского алфавита, цифры, точку, подчеркивание и минус. Почтовый домен должен быть корректным."/>
          </template>
        </n-input>
      </n-auto-complete>
    </n-form-item>
    <n-form-item first label="Пароль" path="password.value">
      <n-input
        type="password"
        show-password-on="click"
        placeholder=""
        v-model:value="formData.password.value">
        <template v-if="!formData.password.isValid" #prefix>
          <question-tooltip text="Пароль должен содержать минимум 8 символов, заглавную букву, строчную букву, цифру и специальный символ."/>
        </template>
      </n-input>
    </n-form-item>
    <n-form-item
      ref="pwdRef"
      first
      label="Повторите пароль"
      path="repeatedPassword.value">
      <n-input
        type="password"
        show-password-on="click"
        placeholder=""
        v-model:value="formData.repeatedPassword.value"/>
    </n-form-item>

    <n-form-item>
      <n-button
        type="primary"
        ghost
        :loading="submitLoading"
        :disabled="!formData.email.isValid || !formData.password.isValid"
        @click="submitForm">Зарегистрироваться</n-button>
    </n-form-item>
  </n-form>
</template>

<style lang="scss" scoped>
.n-input-question:hover {
  color: var(--icon-color-hover);
}
</style>

<script lang="ts">
import { defineComponent } from 'vue'
import { useMessage } from "naive-ui";
import {
  emailRegex,
  passwordRegex,
  externalOptions,
} from "@/variables/auth-vars.js";
import QuestionTooltip from "@/components/QuestionTooltip.vue"

export default defineComponent({
  name: "SignUpForm",
  components: {
    QuestionTooltip
  },
  data() {
    return {
      submitLoading: false,
      message: useMessage(),
      formData: {
        email: {
          value: "",
          isValid: false,
        },
        password: {
          value: "",
          isValid: false,
        },
        repeatedPassword: {
          value: "",
          isValid: false,
        },
      },
      rules: {
        email: {
          value: [
            {
              required: true,
              message: "Пожалуйста, введите email",
              trigger: "blur",
            },
            {
              asyncValidator: (rule, value) => {
                return new Promise((resolve, reject) => {
                  this.formData.email.isValid = false;
                  if (!emailRegex.test(value)) {
                    reject(new Error("Введите корректный email"));
                  } else {
                    this.formData.email.isValid = true;
                    resolve();
                  }
                });
              },
              trigger: ["blur", "input"],
            },
          ],
        },
        password: {
          value: [
            {
              required: true,
              message: "Пожалуйста, введите пароль",
              trigger: "blur",
            },
            {
              asyncValidator: (rule, value) => {
                return new Promise((resolve, reject) => {
                  if (!passwordRegex.test(value)) {
                    this.formData.password.isValid = false;
                    reject(
                      new Error("Введите корректный сложный пароль")
                    );
                  } else {
                    this.formData.password.isValid = true;
                    resolve();
                  }
                });
              },
              trigger: ["blur", "input"],
            },
          ],
        },
        repeatedPassword: {
          value: [
            {
              required: true,
              message: "Пожалуйста, повторите пароль",
              trigger: "blur",
            },
            {
              asyncValidator: (rule, value) => {
                return new Promise((resolve, reject) => {
                  if (value !== this.formData.password.value) {
                    this.formData.repeatedPassword.isValid = false;
                    reject(new Error("Пароли не совпадают"));
                  } else {
                    this.formData.repeatedPassword.isValid = true;
                    resolve();
                  }
                });
              },
              trigger: ["blur", "input"],
            },
          ],
        },
      },
    };
  },
  computed: {
    options() {
      return externalOptions(this.formData.email.value);
    },
  },
  methods: {
    submitForm() {
      this.submitLoading = true;
      this.$refs.formRef.validate((errors) => {
        if (!errors) {
          this.message.success("Valid");
        } else {
          this.message.error("Invalid");
        }
        this.submitLoading = false;
      });
    },
  },
})
</script>
