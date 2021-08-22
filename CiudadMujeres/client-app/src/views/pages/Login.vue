<template>
  <v-container
    id="login"
    class="fill-height justify-center"
    tag="section"
  >
    <v-row justify="center">
      <v-slide-y-transition appear>
        <base-material-card
          color="success"
          light
          max-width="100%"
          width="400"
          class="px-5 py-3"
        >
          <template v-slot:heading>
            <div class="text-center">
              <h1 class="text-h3 font-weight-bold mb-2">
                Inicio de sesión
              </h1>
            </div>
          </template>

          <v-card-text class="text-center">
            <validation-observer v-slot="{ handleSubmit }">
              <form @submit.prevent="handleSubmit(validateForm)">
                <validation-provider
                  v-slot="{ errors }"
                  name="Usuario"
                  rules="required|min:5|max:250"
                >
                  <v-text-field
                    v-model="username"
                    color="secondary"
                    :error-messages="errors"
                    label="Nombre de Usuario..."
                    prepend-icon="mdi-account"
                    class="mt-5"
                  />
                </validation-provider>
                <validation-provider
                  v-slot="{ errors }"
                  name="Password"
                  rules="required|min:5|max:250"
                >
                  <v-text-field
                    v-model="password"
                    :error-messages="errors"
                    class="mb-8"
                    color="secondary"
                    type="password"
                    label="Contraseña..."
                    prepend-icon="mdi-lock-outline"
                  />
                </validation-provider>
                <pages-btn
                  large
                  color=""
                  type="submit"
                  depressed
                  class="v-btn--text success--text"
                >
                  Iniciar Sesión
                </pages-btn>
              </form>
            </validation-observer>
          </v-card-text>
        </base-material-card>
      </v-slide-y-transition>
    </v-row>
  </v-container>
</template>

<script>
  import { required, max, min } from 'vee-validate/dist/rules'
  import { extend, setInteractionMode } from 'vee-validate'

  setInteractionMode('eager')
  extend('max', {
    ...max,
    message: 'El campo {_field_} no debe tener más de {length} caracteres',
  })

  extend('min', {
    ...min,
    message: 'El campo {_field_} debe tener al menos {length} caracteres',
  })

  extend('required', {
    ...required,
    message: 'El campo {_field_} no debe estar vacío',
  })

  export default {
    name: 'PagesLogin',

    $_veeValidate: {
      validator: 'new',
    },

    components: {
      PagesBtn: () => import('./components/Btn'),
    },

    data: () => ({
      username: '',
      password: '',
      socials: [
        {
          href: '#',
          icon: 'mdi-facebook-box',
        },
        {
          href: '#',
          icon: 'mdi-twitter',
        },
        {
          href: '#',
          icon: 'mdi-github-box',
        },
      ],
    }),
    methods: {
      validateForm () {
        this.$http.post('api/security/account/credentials/login', {
          username: this.username,
          password: this.password,
        }).then(response => {
          localStorage.setItem('auth-user', JSON.stringify(response.data))

          this.$router.push({ name: 'User Profile' })
        }).catch(() => {
        })
      },
    },
  }
</script>
