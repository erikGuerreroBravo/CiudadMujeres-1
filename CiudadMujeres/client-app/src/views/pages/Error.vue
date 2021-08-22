<template>
  <v-app id="error">
    <v-container
      class="fill-height"
      fluid
    >
      <v-row
        align="center"
        justify="center"
      >
        <div class="text-md-center">
          <h1 class="errorCode">
            {{ $route.params.errorCode||404 }}
          </h1>
          <h2 class="my-3 text-h3 font-weight-bold">
            {{ errorList[$route.params.errorCode]|| errorList['404'] }}
          </h2>
          <div>
            <v-btn
              color="secondary"
              class="text-h4"
              @click="$router.push({ name: 'Dashboard' })"
            >
              Regresar a la página <br> principal
            </v-btn>
          </div>
        </div>
      </v-row>
    </v-container>
  </v-app>
</template>

<script>
  export default {
    data: () => ({
      errorList: {
        404: 'Página no encontrada',
        401: 'Usted no tiene permisos para esta página',
      },
    }),

  }
</script>

<style scoped>
  .d {
    margin: 0 auto;
    width: 25% !important;
  }
  .errorCode {

    text-align: center;
    vertical-align: middle;
    font-size: 170px;
    line-height: 170px;
    font-weight: 800;
    color: #424242 ;
    text-shadow: rgba(61,61,61,.3) 1px 1px, rgba(61,61,61,.2) 2px 2px, rgba(61,61,61,.3) 3px 3px;
  }
</style>
