import socket

terminate_string = '-term-end-'
BUFF_SIZE = 1024

if __name__ == "__main__":
    with open('lorem_ipsum.txt', 'r') as f:
        fileContents = f.read()

    fileContents += terminate_string

    conn_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    conn_socket.connect(('127.0.0.1', 8080))
    send_len = conn_socket.send(fileContents.encode('utf-8'))

    if send_len != len(fileContents):
        print('error on send')

    result = ''

    while True:
        data = conn_socket.recv(BUFF_SIZE).decode('utf-8')

        if data.find(terminate_string) != -1:
            data = data.replace(terminate_string, '')
            result += data
            break

        result += data
        if len(data) < BUFF_SIZE:
            break

    if result.find(terminate_string) != -1:
        result = result.replace(terminate_string, '')
    print(f'result: {result}')
