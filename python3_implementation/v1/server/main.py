import socket

terminate_string = '-term-end-'
BUFF_SIZE = 10

if __name__ == "__main__":
    server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    server_socket.bind(('127.0.0.1', 8080))
    server_socket.listen(10)

    while True:
        (client_socket, server) = server_socket.accept()

        result = ''

        while True:
            data = client_socket.recv(BUFF_SIZE).decode('utf-8')

            print(f'    {data}')
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

        sendData = f'received {len(result)} bytes' + terminate_string
        send_len = client_socket.send(sendData.encode('utf-8'))

        if send_len != len(sendData):
            print('error on send')

        client_socket.close()
