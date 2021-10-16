export interface FormItemData<Type extends string | boolean> {
    value: Type,
    isValid?: boolean
}

export interface SignUpFormData {
    email: FormItemData<string>,
    password: FormItemData<string>,
    confirmedPassword: FormItemData<string>
}

export interface SignInFormData {
    email: FormItemData<string>,
    password: FormItemData<string>,
    remembered: FormItemData<boolean>
} 
