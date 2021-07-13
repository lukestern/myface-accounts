import React, {createContext, ReactNode, useState} from "react";

export const LoginContext = createContext({
    authHeader: "",
    isLoggedIn: false,
    isAdmin: false,
    logIn: (value: string) => {},
    logOut: () => {},
});

interface LoginManagerProps {
    children: ReactNode
}

export function LoginManager(props: LoginManagerProps): JSX.Element {
    const [loggedIn, setLoggedIn] = useState(false);
    const [authHeader, setAuthHeader] = useState("");
    
    function logIn(newAuthHeader: string) {
        setLoggedIn(true);
        setAuthHeader(newAuthHeader);
    }
    
    function logOut() {
        setLoggedIn(false);
        setAuthHeader("");
    }
    
    const context = {
        authHeader: authHeader,
        isLoggedIn: loggedIn,
        isAdmin: loggedIn,
        logIn: logIn,
        logOut: logOut,
    };
    
    return (
        <LoginContext.Provider value={context}>
            {props.children}
        </LoginContext.Provider>
    );
}