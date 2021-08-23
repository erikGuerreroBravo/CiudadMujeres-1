import Authorization from './Authorization'

export default function UserHasPermissions (router) {
    router.beforeEach((to, from, next, abort, redirect) => {
        let authorized = false

        if (to.meta !== {}) {
            authorized = Authorization.authorize(
                to.meta.requiresLogin,
                to.meta.requiredPermissions,
                to.meta.permissionType,
            )

            if (authorized === 'loginIsRequired') {
                router.push({ name: 'Login' }).catch((errors) => {
                  })
            }

            if (authorized === 'notAuthorized') {
                router.push({ name: 'Error', params: { errorCode: 401 } }).catch((errors) => {
                  })
            }
        }
        next()
    })
}
