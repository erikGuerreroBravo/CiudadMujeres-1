/**
 * This is where all the authorization login is stored
 */
import Authorization from './Authorization'

export default function UserHasPermissions (router) {
    /**
     * Before each route we will see if the current user is authorized
     * to access the given route
     */
    router.beforeEach((to, from, next, abort, redirect) => {
        let authorized = false

        /**
         * Remember that access object in the routes? Yup this why we need it.
         *
         */
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
                /**
                 * Redirects to a "default" page
                 */
                router.push({ name: 'Error', params: { errorCode: 401 } }).catch((errors) => {
                  })
            }
        }
        /**
         * Everything is fine? Let's to the page then.
         */
        next()
    })
}
