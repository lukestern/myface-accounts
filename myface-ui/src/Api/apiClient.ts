export interface ListResponse<T> {
    items: T[];
    totalNumberOfItems: number;
    page: number;
    nextPage: string;
    previousPage: string;
}

export interface Login {
    id: number;
    username: string;
    isLoggedIn: boolean;
}

export interface User {
    id: number;
    firstName: string;
    lastName: string;
    displayName: string;
    username: string;
    email: string;
    profileImageUrl: string;
    coverImageUrl: string;
}

export interface Interaction {
    id: number;
    user: User;
    type: string;
    date: string;
}

export interface Post {
    id: number;
    message: string;
    imageUrl: string;
    postedAt: string;
    postedBy: User;
    likes: Interaction[];
    dislikes: Interaction[];
}

export interface NewPost {
    message: string;
    imageUrl: string;
}


export async function fetchLogin(encodedAuth: string): Promise<Login> {
    const response = await fetch(`https://localhost:5001/users`, {
        headers: {
            'Authorization': `Basic ${encodedAuth}`
        }
    });
    return await response.json();
}

export async function fetchUsers(searchTerm: string, page: number, pageSize: number, encodedAuth: string): Promise<ListResponse<User>> {
    const response = await fetch(`https://localhost:5001/users?search=${searchTerm}&page=${page}&pageSize=${pageSize}`, {
        headers: {
            'Authorization': `Basic ${encodedAuth}`
        }
    });
    return await response.json();
}

export async function fetchUser(userId: string | number, encodedAuth: string): Promise<User> {
    const response = await fetch(`https://localhost:5001/users/${userId}`, {
        headers: {
            'Authorization': `Basic ${encodedAuth}`
        }
    });
    return await response.json();
}

export async function fetchPosts(page: number, pageSize: number, encodedAuth: string): Promise<ListResponse<Post>> {
    const response = await fetch(`https://localhost:5001/feed?page=${page}&pageSize=${pageSize}`, {
        headers: {
            'Authorization': `Basic ${encodedAuth}`
        }
    });
    return await response.json();
}

export async function fetchPostsForUser(page: number, pageSize: number, userId: string | number, encodedAuth: string) {
    const response = await fetch(`https://localhost:5001/feed?page=${page}&pageSize=${pageSize}&postedBy=${userId}`, {
        headers: {
            'Authorization': `Basic ${encodedAuth}`
        }
    });
    return await response.json();
}

export async function fetchPostsLikedBy(page: number, pageSize: number, userId: string | number, encodedAuth: string) {
    const response = await fetch(`https://localhost:5001/feed?page=${page}&pageSize=${pageSize}&likedBy=${userId}`, {
        headers: {
            'Authorization': `Basic ${encodedAuth}`
        }
    });
    return await response.json();
}

export async function fetchPostsDislikedBy(page: number, pageSize: number, userId: string | number, encodedAuth: string) {
    const response = await fetch(`https://localhost:5001/feed?page=${page}&pageSize=${pageSize}&dislikedBy=${userId}`, {
        headers: {
            'Authorization': `Basic ${encodedAuth}`
        }
    });
    return await response.json();
}

export async function createPost(newPost: NewPost, encodedAuth: string) {
    const response = await fetch(`https://localhost:5001/posts/create`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            'Authorization': `Basic ${encodedAuth}`
        },
        body: JSON.stringify(newPost),
    });

    if (!response.ok) {
        throw new Error(await response.json())
    }
}
