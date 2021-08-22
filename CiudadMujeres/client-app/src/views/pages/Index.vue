<template>
  <v-app>
    <pages-core-app-bar />

    <pages-core-view :img="img" />

    <pages-core-footer />
  </v-app>
</template>

<script>
  export default {
    name: 'PagesIndex',

    components: {
      PagesCoreAppBar: () => import('./components/core/AppBar'),
      PagesCoreFooter: () => import('./components/core/Footer'),
      PagesCoreView: () => import('./components/core/View'),
    },

    data: () => ({
      img: '',
    }),
    watch: {
      $route (to, from) {
        this.img = to.name === 'Login' ? 'lock.jpg' : ''
      },
    },

    created () {
      this.img = 'lock.jpg'
      this.$vuetify.theme.dark = true
    },

    beforeDestroy () {
      this.$vuetify.theme.dark = false
    },
  }
</script>
