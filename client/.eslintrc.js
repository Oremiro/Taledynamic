module.exports = {
	root: true,
	env: {
		node: true,
    'vue/setup-compiler-macros': true
	},
  plugins: [
    '@typescript-eslint',
  ],
  extends: [
    'eslint:recommended',
    'plugin:vue/vue3-recommended',
		'plugin:@typescript-eslint/recommended',
    'prettier'
  ],
	parser: 'vue-eslint-parser',
	parserOptions: {
		parser: '@typescript-eslint/parser',
		sourceType: 'module'
	},
  rules: {
    "@typescript-eslint/no-empty-interface": [
      "error",
      {
        "allowSingleExtends": true
      }
    ]
  }
}